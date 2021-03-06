﻿
using System;
using UnityEngine;
using System.Reflection;

namespace Hananoki.GitHubDownload {
	public static class JsonHelper {
		public static T[] FromJson<T>( string json ) {
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>( json );
			return wrapper.Items;
		}
		public static string ToJson<T>( T[] array ) {
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson( wrapper );
		}
		public static string ToJson<T>( T[] array, bool prettyPrint ) {
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson( wrapper, prettyPrint );
		}
		[Serializable]
		private class Wrapper<T> {
			public T[] Items;
		}
	}

	public static class ManifestJson {
		static Type s_UnityEditor_Json;
		static Type UnityEditor_Json {
			get {
				if( s_UnityEditor_Json == null) {
					s_UnityEditor_Json = Assembly.Load( "UnityEditor.dll" ).GetType( "UnityEditor.Json" );
				}
				return s_UnityEditor_Json;
			}
		}
		static MethodInfo s_UnityEditor_Json_Deserialize;
		static MethodInfo UnityEditor_Json_Deserialize {
			get {
				if( s_UnityEditor_Json_Deserialize == null ) {
					s_UnityEditor_Json_Deserialize = UnityEditor_Json.GetMethod( "Deserialize", flag );
				}
				return s_UnityEditor_Json_Deserialize;
			}
		}
		static MethodInfo s_UnityEditor_Serialize;
		static MethodInfo UnityEditor_Serialize {
			get {
				if( s_UnityEditor_Serialize == null ) {
					s_UnityEditor_Serialize = UnityEditor_Json.GetMethod( "Serialize", flag );
				}
				return s_UnityEditor_Serialize;
			}
		}

		const BindingFlags flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public static object Deserialize( string json ) {
			return UnityEditor_Json_Deserialize.Invoke( null, new object[] { json } );
		}
		public static string Serialize( object obj, bool pretty = false, string indentText = "  " ) {
#if UNITY_2019_1_OR_NEWER
			return (string) UnityEditor_Serialize.Invoke( null, new object[] { obj, pretty, indentText } );
#else
			return (string) UnityEditor_Serialize.Invoke( null, new object[] { obj } );
#endif
		}
	}
}