// Copyright (c) 2023 Frederick William Haslam born 1962 in the USA.

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Global {

	/// <summary>
	/// Utility using Coroutines to ensure classes are ready for each other.
	/// Clases will call Ready(this) when they are ready ( eg. enabled )
	/// Classes will call NotReady(this) when they are no longer ready ( eg. disabled )
	/// 
	/// Classes that are delaying readiness will call:
	///		WaitFor( this, comma separated class types as params )
	///		WaitFor( this, readyAction, comma separated class types as params )
	///		
	/// This is a global, persistent class.  Coroutines are run under this class 
	///		and ocucr between Update and LateUpdate
	/// This permits the 'WaitFor()' functions to disable/enable the script.
	///		see: https://docs.unity3d.com/Manual/ExecutionOrder.html
	///		
	/// </summary>
	public class ReadyWait : SingletonPersistentMonoBehaviour<ReadyWait> {

		// These classes have finished readying themselves
		internal static Dictionary<Type,object> readyClasses = new Dictionary<Type,object>();

		// Coroutines waiting for StartCoroutine()
		internal static Queue<IEnumerator> readyQueue = new Queue<IEnumerator>();

		// Types currently waiting as Coroutines
		internal static Dictionary<Type,Type> waitNeeds = new Dictionary<Type, Type>();

		// periodically check for missing needs
		internal readonly static float STATUS_CHECK_REPEAT_SECONDS = 10f;
		internal static float nextStatusCheck;


		protected override void Initialize() { 
			nextStatusCheck = Time.time + STATUS_CHECK_REPEAT_SECONDS;
		}

		internal void Update() {

			while ( readyQueue.Count>0 ) {
				StartCoroutine( readyQueue.Dequeue() );
			}

			// periodic status check, let us know what is missing
			if ( Time.time >= nextStatusCheck ) {
				nextStatusCheck += STATUS_CHECK_REPEAT_SECONDS;
				if (waitNeeds.Count>0) {
					Debug.Log("ReadyWait.Update()  Status="+StatusDisplay());
				}
			}
		}
		
		/// <summary>
		/// Used for debugging
		/// </summary>
		/// <returns></returns>
		public static string StatusDisplay(){
			
			var waiting = waitNeeds.Select( (e) => (e.Key.ToString()+">"+e.Value.ToString()) );
			var ready = readyClasses.Keys.Select( (k) => k.ToString() );

			return "ReadyStatus[ " +
				"Wait=(\n\t\t"+string.Join("\n\t\t",waiting)+")" +
				"\n\tReady=(\n\t\t"+string.Join("\n\t\t",ready)+") ]";
		}

//======================================================================================================================

		public static void Ready( object who ) {
			var type = who.GetType();
			readyClasses[type] = who;
	Debug.Log("ReadyWait.Ready = " + type);
		}

		public static void NotReady( object who ) {
			var type = who.GetType();
			if (readyClasses.ContainsKey(type)) 
				readyClasses.Remove(type);
		}

		public static T GetReady<T>( Type type ) where T : class {
			if (!readyClasses.ContainsKey(type)) return null;
			return (T)readyClasses[type];
		}

		public static void WaitFor( MonoBehaviour owner, params Type[] needs ) {
			WaitFor( owner, null, needs );
		}

		public static void WaitFor( MonoBehaviour owner, Action readyAction, params Type[] needs ) {
			NotReady( owner );
			waitNeeds[owner.GetType()] = typeof(object);
			readyQueue.Enqueue( WaitCoroutine( owner, readyAction, needs ) );
		}

		internal static IEnumerator WaitCoroutine( MonoBehaviour owner, Action readyAction, params Type[] needs ) {

			foreach ( var need in needs ) {
//if (owner.GetType()==typeof(ToolBlocksPanelController)) {
//Debug.Log(">>>> ToolBlocksPanelsController need ={ "+need );
//Debug.Log("DISPLAY="+ReadyStatusDisplay());
//Debug.Log("Contains="+( readyClasses.ContainsKey(need) ) );
//}
				waitNeeds[owner.GetType()] = need;
				yield return new WaitUntil( () => readyClasses.ContainsKey( need ) );
			}
			waitNeeds.Remove( owner.GetType() );

			try { if (readyAction!=null) readyAction(); }
			catch (Exception ex){ Debug.LogException( ex ); }

			Ready( owner );
		}
	}

}