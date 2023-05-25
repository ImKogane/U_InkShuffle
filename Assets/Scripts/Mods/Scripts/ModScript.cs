using System.IO;
using JetBrains.Annotations;
using MoonSharp.Interpreter;
using UnityEngine;
using Utilities;

namespace Mods
{
	public abstract class ModScript : Script
	{
		public static readonly string basePath = Application.streamingAssetsPath + "/Mods/";
		public static readonly string scriptExtension = ".lua";
		public static readonly string imageExtension = ".png";
		
		protected Mod _mod;
		public Mod GetOwningMod() => _mod;

		public ModScript(Mod mod, CoreModules coreModules) : base(coreModules)
		{
			_mod = mod;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		private void LuaErrorMessage(InterpreterException e)
		{
			Debug.LogWarning("Lua error detected for mod \"" + _mod.ModName + "\", reason: " + e.DecoratedMessage);
		}
		
		// Code candy
		public bool TryDoFile(string fileName)
		{
			string fullPath = basePath + _mod.ModName + "/" + fileName + scriptExtension;
			return TryLuaRunner(() => DoString(File.ReadAllText(fullPath)));
		}
		
		/// <summary>
		/// Try to call a Runner (Action).<br/>
		/// Meant to be used as a proxy for Script functions that can throw Lua errors.
		/// </summary>
		/// <param name="runner">The function.</param>
		/// <returns>
		/// <b>True</b> if the call succeeded.<br/>
		/// <b>False</b> if the call failed, and outputs a warning if it was due to a Lua error.
		/// </returns>
		public bool TryLuaRunner(Runner runner)
		{
			try
			{
				runner.Invoke();
				return true;
			}
			catch (InterpreterException e)
			{
				LuaErrorMessage(e);
				return false;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// Try to call a Supplier (Func).<br/>
		/// Meant to be used as a proxy for Script functions that can throw Lua errors.
		/// </summary>
		/// <param name="supplier">The function.</param>
		/// <returns>
		/// <b>T</b> if the call succeeded.<br/>
		/// <b>default(T)</b> if the call failed, and outputs a warning if it was due to a Lua error.
		/// </returns>
		[CanBeNull]
		public T TryLuaSupplier<T>(Supplier<T> supplier)
		{
			try
			{
				return supplier.Invoke();
			}
			catch (InterpreterException e)
			{
				LuaErrorMessage(e);
				return default;
			}
			catch
			{
				return default;
			}
		}
		
	}
}
