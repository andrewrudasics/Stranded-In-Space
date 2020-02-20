// Copyright 2017 Hans Jorgensen
// Permission is granted to distribute, modify, and use
// in CSE 481 D capstone projects

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.Networking;

namespace cse481.logging {
	
	/// <summary>
	/// Logs level and action information for CSE481D capstone projects
    /// using the Unity game engine
	/// </summary>
    /// <remarks>
    /// This class is designed to work with the Unity Game Engine,
    /// but is not itself a script. To use it in Unity projects,
    /// keep a static singleton instance of this class in a <see cref="MonoBehaviour"/>
    /// that performs game-specific logging for each object.
    /// 
    /// <para>The StartNewSession and LogLevelStart logging methods are coroutines designed to
    /// work with <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/>,
    /// which coroutines will wait until the server request either
    /// completes or encounters an error. All others are regular function calls that do not
	/// require waiting for response data to come back from the http endpoint</para>
    /// </remarks>
	public class CapstoneLogger 
	{
		const string prdUrl = "https://integration.centerforgamescience.org/cgs/apps/games/v2/index.php/";

		/*
		 * Properties specific to each game
		 */
		private int gameId;
		private string gameName;
		private string gameKey;
		private int categoryId;
		private int versionNumber;

		/*
		 * Logging state
		 */
		private string currentUserId;
		private string currentSessionId;
		private string currentDqid;
		private int currentLevelId;

		private int currentLevelSeqInSession;
		private int currentActionSeqInSession;
		private int currentActionSeqInLevel;

		private DateTime timestampOfPrevLevelStart;
		private DateTime timestampOfPrevAction = DateTime.Now;

		private List<LevelAction> levelActionBuffer;

		private MD5 md5;

		private List<UnityWebRequest> bufferedRequestsWaitingForSession;

		/// <summary>
		/// Initializes a new instance of the <see cref="cse481.logging.CapstoneLogger"/> class.
		/// </summary>
		/// <param name="gameId">Game identifier. Use the one assigned to your team.</param>
		/// <param name="gameName">Game name. Use the one assigned to your team.</param>
		/// <param name="gameKey">Game key. Use the one assigned to your team.</param>
		/// <param name="categoryId">A number you assign to help categorize data from
		/// different releases of your game (i.e. debug vs. friends vs. public)</param>
		public CapstoneLogger(int gameId, string gameName, string gameKey, 
				int categoryId) 
		{
			this.gameId = gameId;
			this.gameName = gameName;
			this.gameKey = gameKey;
			this.categoryId = categoryId;
			this.versionNumber = 1;

			this.levelActionBuffer = new List<LevelAction>();
		}

		/// <summary>
		/// Generate a new UUID to identify the user
		/// </summary>
		/// <returns>The generated UUID as a string.</returns>
		public string GenerateUuid()
		{
            string generatedID = Guid.NewGuid().ToString();
            Debug.Log("Generated ID: " + generatedID);
			return generatedID;
		}

		/// <summary>
		/// Get either the saved UserID or null if none has been saved
		/// </summary>
		/// <returns>The saved user identifier.</returns>
		public string GetSavedUserId()
		{
            string userID = PlayerPrefs.GetString("UserID", null);
            Debug.Log("Retrieving User ID: " + userID);
            return userID;
		}

		/// <summary>
		/// Set the saved UserID to the given value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetSavedUserId(string value)
		{
            Debug.Log("Setting User ID: " + value);
			PlayerPrefs.SetString ("UserID", value);
            PlayerPrefs.Save();
		}

		public bool InSession {
			get {
				return currentSessionId != null;
			}
		}

		/// <summary>
		/// Coroutine that begins logging for a new session
		/// by establishing contact with the server.
		/// </summary>
		/// <returns>An IEnumerator to pass into StartCoroutine</returns>
		/// <param name="userId">The retrieved user identifer to use for the session.</param>
		public IEnumerator StartNewSession(string userId)
		{
			this.currentUserId = userId;
			this.currentLevelSeqInSession = 0;
			this.currentActionSeqInSession = 0;

			var sessionParams = new SessionParams() {
				eid = 0,
				cid = this.categoryId,
				pl_detail = new SessionParams.PlayerDetail(),
				client_ts = DateTime.Now.TotalMilliseconds(),
				uid = this.currentUserId,
				g_name = this.gameName,
				gid = this.gameId,
				svid = 2,
				vid = this.versionNumber
			};

			Dictionary<string, string> requestParams = PrepareParams(sessionParams);
			UnityWebRequest sessionRequest = PrepareRequest("loggingpageload/set/", requestParams);
			yield return sessionRequest.SendWebRequest();
			if (!sessionRequest.isNetworkError) {
				// Return data formatted like data={...}
				string text = sessionRequest.downloadHandler.text.Substring (5);
				Debug.Log (text);
                var parsedResults = JsonUtility.FromJson<NewSessionResponse>(text);
				if (parsedResults.tstatus == "t") {
					this.currentSessionId = parsedResults.r_data.sessionid;
				}
			} else {
				Debug.Log (sessionRequest.error);
			}
		}

        [Serializable]
        private class SessionParams
        {
            public int eid;
            public int cid;
            public PlayerDetail pl_detail;
            public double client_ts;
            public string uid;
            public string g_name;
            public int gid;
            public int svid;
            public int vid;

            [Serializable]
            public class PlayerDetail
            {
                // no fields
            }
        }

        [Serializable]
        private class NewSessionResponse
        {
            public string tstatus;
            public R_Data r_data;

            [Serializable]
            public class R_Data
            {
                public string sessionid;
            }
        }

		/// <summary>
		/// Coroutine that begins logging for a new level
		/// </summary>
		/// <returns>An IEnumerator to pass into StartCoroutine</returns>
		/// <param name="levelId">A number corresponding to the current game level.
		/// This number should be unique across all levels and remain consistent
		/// across sessions.</param>
		/// <param name="details">Extra details associated with the level start</param>
		public IEnumerator LogLevelStart(int levelId, string details)
		{
			this.FlushBufferedLevelActions();

			this.timestampOfPrevLevelStart = DateTime.Now;
			this.currentActionSeqInLevel = 0;
			this.currentLevelId = levelId;
			this.currentDqid = null;

			var startData = this.GetCommonData<LevelStartData>();
			startData.sessionid = this.currentSessionId;
			startData.sid = this.currentSessionId;
			startData.quest_seqid = ++this.currentLevelSeqInSession;
			startData.qaction_seqid = ++this.currentActionSeqInLevel;
			startData.q_detail = details;
			startData.q_s_id = 1;
			startData.session_seqid = ++this.currentActionSeqInSession;

			Dictionary<string, string> requestParams = PrepareParams (startData);
			UnityWebRequest levelStartRequest = PrepareRequest("quest/start/", requestParams);

			yield return levelStartRequest.SendWebRequest();

			if (!levelStartRequest.isNetworkError == true) {
				string text = levelStartRequest.downloadHandler.text.Substring (5);
				Debug.Log (text);
                this.currentDqid = JsonUtility.FromJson<LevelStartResponse>(text).dqid;
			} else {
				Debug.Log (levelStartRequest.error);
			}
		}

        [Serializable]
        private class LevelStartData : CommonLogData
        {
            public string sessionid;
            public int quest_seqid;
            public int qaction_seqid;
            public string q_detail;
            public int q_s_id;
            public int session_seqid;
        }

        [Serializable]
        private class LevelStartResponse
        {
            public string dqid;
        }

		/// <summary>
		/// Function that logs the end of the level.
		/// </summary>
		/// <param name="details">Any details about the ending of the level
		/// that you want to record.</param>
		public void LogLevelEnd(string details)
		{
			this.FlushBufferedLevelActions();

			var endData = this.GetCommonData<LevelEndData>();
			endData.sessionid = this.currentSessionId;
			endData.sid = this.currentSessionId;
			endData.qaction_seqid = ++this.currentActionSeqInLevel;
			endData.q_detail = details;
			endData.q_s_id = 0;
			endData.dqid = this.currentDqid;
			endData.session_seqid = ++this.currentActionSeqInSession;

			Dictionary<string, string> requestParams = PrepareParams (endData);
			UnityWebRequest levelEndRequest = PrepareRequest("quest/end/", requestParams);
			levelEndRequest.SendWebRequest();

			this.currentDqid = null;
		}

        [Serializable]
        private class LevelEndData : CommonLogData
        {
            public string sessionid;
            public int qaction_seqid;
            public string q_detail;
            public int q_s_id;
            public string dqid;
            public int session_seqid;
        }
        
        /// <summary>
        /// Function that logs an action taken in the level.
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="actionId">A number that you assign that maps
        /// to a particular action that you are interested in recording.
        /// Each action type should have a unique number that is kept
        /// consistent across sessions.</param>
        /// <param name="details">Any details associated with the action
        /// (e.g. location, target, success, etc.)</param>
        /// <remarks>Because it is expected that the game will log player
        /// actions frequently, level actions are buffered and are
        /// flushed in batches to the server as the implementation
        /// sees fit. Level actions will also be flushed when
        /// <see cref="LogLevelEnd(string)"/> or <see cref="FlushBufferedLevelActions"/>
        /// are called. </remarks>
        public void LogLevelAction(int actionId, string details)
		{
			// Per action, figure out the time since the start of the level
			DateTime timestampOfAction = DateTime.Now;
			TimeSpan relativeTime = timestampOfAction - this.timestampOfPrevLevelStart;
			var individualAction = new LevelAction {
				detail = details,
				client_ts = timestampOfAction.TotalMilliseconds(),
				ts = relativeTime.TotalMilliseconds,
				te = relativeTime.TotalMilliseconds,
				session_seqid = ++this.currentActionSeqInSession,
				qaction_seqid = ++this.currentActionSeqInLevel,
				aid = actionId
			};
			this.levelActionBuffer.Add(individualAction);

			Debug.Log("Level action " + this.currentDqid + " " + actionId);

			// Flush buffer if it exceeds a certain length or some amount of time has elapsed since
			// the last time we flushed the buffer.
			double timeSinceLastAction = (timestampOfAction - this.timestampOfPrevAction).TotalMilliseconds;
            if (levelActionBuffer.Count >= 5 || timeSinceLastAction > 2000)
            {
                this.FlushBufferedLevelActions();
            }
			this.timestampOfPrevAction = DateTime.Now;
		}

        [Serializable]
        private class LevelAction
        {
            public string detail;
            public string sid;
            public double client_ts;
            public double ts;
            public double te;
            public int session_seqid;
            public int qaction_seqid;
            public int aid;
        }

		/// <summary>
		/// Function logs an action with no associated level.
		/// </summary>
        /// <returns>nothing</returns>
		/// <param name="actionId">A number that you assign that maps
		/// to a particular action that you are interested in recording.
		/// Each action type should have a unique number that is kept
		/// consistent across sessions.</param>
		/// <param name="details">Any details associated with the action
		/// (e.g. location, target, success, etc.)</param>
		/// <remarks>Unlike level actions, non-level actions are unbuffered
		/// and sent to the server immmediately without needing to be flushed.</remarks>
		public void LogActionWithNoLevel(int actionId, string details)
		{
            var actionNoLevelData = new ActionNoLevelData {
                session_seqid = ++this.currentActionSeqInSession,
                cid = this.categoryId,
                client_ts = DateTime.Now.TotalMilliseconds(),
                aid = actionId,
                vid = this.versionNumber,
                uid = this.currentUserId,
                g_name = this.gameName,
                a_detail = details,
                gid = this.gameId,
                svid = 2,
                sessionid = this.currentSessionId
            };
			Dictionary<string, string> requestParams = PrepareParams (actionNoLevelData);
			UnityWebRequest actionNoLevelRequest = PrepareRequest("loggingactionnoquest/set/", requestParams);
			actionNoLevelRequest.SendWebRequest();
		}

        [Serializable]
        private class ActionNoLevelData
        {
            public int session_seqid;
            public int cid;
            public double client_ts;
            public int aid;
            public int vid;
            public string uid;
            public string g_name;
            public string a_detail;
            public int gid;
            public int svid;
            public string sessionid;
        }

		/// <summary>
		/// Coroutine that flushes the level actions to the server
		/// </summary>
        /// <returns>An IEnumerator to pass into StartCoroutine</returns>
		/// <remarks>
		/// While most logging events in this API are unbuffered, it is expected
		/// that you will be logging actions very frequently. As such, the
		/// actions are buffered until you either call this method or start or end
		/// a level.
		/// </remarks>
		public void FlushBufferedLevelActions()
		{
			// Don't log any actions until a dqid has been set
			if (this.levelActionBuffer.Count > 0 && this.currentDqid != null)
			{
                Debug.Log("Flushing " + this.levelActionBuffer.Count + " actions");
				var bufferedActionsData = this.GetCommonData<LevelActionsLogData>();
				bufferedActionsData.actions = this.levelActionBuffer;
				bufferedActionsData.dqid = this.currentDqid;

				Dictionary<string, string> requestParams = PrepareParams (bufferedActionsData);
				UnityWebRequest levelActionRequest = 
					PrepareRequest("logging/set/", requestParams);

				levelActionRequest.SendWebRequest();

				// Clear out old array
				this.levelActionBuffer = new List<LevelAction>();
			}
		}

        [Serializable]
        private class LevelActionsLogData : CommonLogData
        {
            public List<LevelAction> actions;
            public string dqid;
        }

		private UnityWebRequest PrepareRequest(string suffix, Dictionary<string, string> parameters){
            UnityWebRequest request = UnityWebRequest.Post(ComposeUrl(suffix), parameters);

			return request;
		}

		private string ComposeUrl(string suffix)
		{
			string baseUrl = CapstoneLogger.prdUrl;
			return baseUrl + suffix;
		}

		private T GetCommonData<T>() where T : CommonLogData, new()
		{
			return new T {
                client_ts = (DateTime.Now - DateTime.MinValue).TotalMilliseconds,
				cid = this.categoryId,
				svid = 2,
				lid = 0,
				vid = this.versionNumber,
				qid = this.currentLevelId,
				g_name = this.gameName,
				uid = this.currentUserId,
                sid = this.currentSessionId,
				g_s_id = this.gameId,
				tid = 0,
				gid = this.gameId
			};
		}

        [Serializable]
        private abstract class CommonLogData
        {
            public double client_ts;
            public int cid;
            public int svid;
            public int lid;
            public int vid;
            public int qid;
            public string g_name;
            public string uid;
            public string sid;
            public int g_s_id;
            public int tid;
            public int gid;
        }

		private Dictionary<string, string> PrepareParams(object data)
		{
            // Standard template data sent for every request
            string stringifiedData = (data != null) ? JsonUtility.ToJson(data) : "null";
			Dictionary<string, string> requestBlob = new Dictionary<string, string>
			{
				{"dl", "0"},
				{"latency", "5"},
				{"priority", "1"},
				{"de", "0"},
				{"noCache", ""},
				{"cid", this.categoryId.ToString() },
				{"gid", this.gameId.ToString() },
				{"data", stringifiedData},
				{"skey", this.EncodedData(stringifiedData) }
			};

			return requestBlob;
		}

		private string EncodedData(string value)
		{
			if (value == null)
			{
				value = "";
			}
			string salt = value + this.gameKey;

			MD5 md5 = System.Security.Cryptography.MD5.Create ();
			byte[] saltBytes = System.Text.Encoding.ASCII.GetBytes (salt);
			byte[] hash = md5.ComputeHash (saltBytes);

			string result = "";
			foreach (byte b in hash) {
				result += b.ToString ("x2");
			}
			return result;
		}
	}

    static class DateTimeExtensions
    {
        public static double TotalMilliseconds(this DateTime datetime)
        {
            return (double)(datetime.Ticks / 10000);
        }
    }
}