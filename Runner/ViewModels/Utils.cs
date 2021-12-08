using System;
using System.Reflection;

namespace Runner.ViewModels
{
	/// <summary>
	/// Методы для упрощения работы с отражением
	/// </summary>
	public class UtilsReflection
	{ 

		/// <summary>
		/// Получает значение static поля (в том числе сложного)
		/// </summary>
		/// <param name="typ">Тип-владелец статического поля</param>
		/// <param name="propertyPath">Название свойства или путь</param>
		/// <returns></returns>
		public static object? GetFieldValue(Type? typ, string propertyPath)
		{
			if (typ != null)
			{
				var pName = propertyPath.Split('.')[0];
				var fi = typ.GetField(pName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (fi != null)
				{
					var o = fi.GetValue(null);

					if (pName == propertyPath)
					{
						return o;
					}
					else
					{
						return GetFieldValue(o?.GetType(), propertyPath.Substring(pName.Length + 1));
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Получает значение поля (в том числе сложного)
		/// </summary>
		/// <param name="obj">Расширяемый объект</param>
		/// <param name="propertyPath">Название свойства или путь</param>
		/// <returns></returns>
		public static object? GetFieldValue(object? obj, string propertyPath)
		{
			if (obj != null)
			{
				var pName = propertyPath.Split('.')[0];
				var fi = obj.GetType().GetField(pName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (fi != null)
				{
					var o = fi.GetValue(obj);

					if (pName == propertyPath)
					{
						return o;
					}
					else
					{
						return GetFieldValue(o, propertyPath.Substring(pName.Length + 1));
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Получает значение свойства (в том числе сложного)
		/// </summary>
		/// <param name="obj">Расширяемый объект</param>
		/// <param name="propertyPath">Название свойства или путь</param>
		public static object? GetPropertyValue(object? obj, string propertyPath)
		{
			if (obj != null)
			{
				var pName = propertyPath.Split('.')[0];
				var pi = obj.GetType().GetProperty(pName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (pi != null)
				{
					var o = pi.GetValue(obj, null);

					if (pName == propertyPath)
					{
						return o;
					}
					else
					{
						return GetPropertyValue(o, propertyPath.Substring(pName.Length + 1));
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Устанавливаен значение свойства (в том числе сложного)
		/// </summary>
		/// <param name="obj">Расширяемый объект</param>
		/// <param name="propertyPath">Название свойства или путь</param>
		/// <param name="value">Знсчение</param>
		public static void SetPropertyValue(object? obj, string propertyPath, object value)
		{
			if (obj != null)
			{
				var pName = propertyPath.Split('.')[0];
				var pi = obj.GetType().GetProperty(pName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (pi != null)
				{
					if (pName == propertyPath)
					{
						pi.SetValue(obj, value, null);
					}
					else
					{
						var o = pi.GetValue(obj, null);
						SetPropertyValue(o, propertyPath.Substring(pName.Length + 1), value);
					}
				}
			}
		}

	}

}
