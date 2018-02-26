using System.Web.Mvc;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Shop.Web.Core.Helper
{
    public static class EnumHelper
    {

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, bool markCurrentAsSelected = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         select new {
                             ID = Convert.ToInt32(enumValue),
                             Name = (typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute)!=null?
                            ((typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute)).GetName() : Enum.GetName(typeof(TEnum), enumValue)
                         };
           
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }
        public static IEnumerable<SelectListItem> ToSelectListItem<TEnum>(this TEnum enumObj, TEnum selected) where TEnum : struct
        {
            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         select new SelectListItem
                         {
                             Value = Convert.ToInt32(enumValue).ToString(),
                             Text = (typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute) != null ?
                            ((typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute)).GetName() : Enum.GetName(typeof(TEnum), enumValue),
                             Selected = Convert.ToInt32(enumValue) == Convert.ToInt32(selected)

                         };
            return values;
        }
        
        public static SelectList ToSelectListWithName<TEnum>(this TEnum enumObj, bool markCurrentAsSelected = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         select new
                         {
                             ID = Enum.GetName(typeof(TEnum), enumValue),
                             Name = (typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute) != null ?
                            ((typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute)).GetName() : Enum.GetName(typeof(TEnum), enumValue)
                         };

            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }
        public static SelectList ToSelectName<TEnum>(this TEnum enumObj, bool markCurrentAsSelected = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

            var values = from TEnum enumValue in Enum.GetValues(typeof (TEnum))
                         let displayAttribute = typeof (TEnum).GetField(enumValue.ToString()).GetCustomAttributes(
                             typeof (DisplayAttribute), false).FirstOrDefault() as DisplayAttribute
                         where displayAttribute != null
                         select new
                             {
                                 ID =
                             displayAttribute != null
                                 ? displayAttribute.
                                       GetName()
                                 : Enum.GetName(typeof (TEnum), enumValue),
                                 Name =
                             displayAttribute != null
                                 ? displayAttribute.
                                       GetName()
                                 : Enum.GetName(typeof (TEnum), enumValue)
                             };

            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }


        public static string GetDescription<TEnum>(this TEnum enumObj, Enum en)
        {
            return (typeof(TEnum).GetField(en.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute) != null ?
                            ((typeof(TEnum).GetField(en.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute)).GetName() : Enum.GetName(typeof(TEnum), en);
        }


        public static T ConvertToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        //public static string GetDescription<T>(string value) where T : struct
        //{
        //    T en;
        //    if (!Enum.TryParse(value, true, out en))
        //        return "Unknown";
        //    return EnumHelper.GetDescription(en as Enum) ?? "Unknown";
        //}

        //public string GetDisplayName(object obj, string propertyName)
        //{
        //    if (obj == null) return null;
        //    return GetDisplayName(obj.GetType(), propertyName);

        //}

        //public string GetDisplayName(Type type, string propertyName)
        //{
        //    var property = type.GetProperty(propertyName);
        //    if (property == null) return null;

        //    return GetDisplayName(property);
        //}

        //public string GetDisplayName(PropertyInfo property)
        //{
        //    var attrName = GetAttributeDisplayName(property);
        //    if (!string.IsNullOrEmpty(attrName))
        //        return attrName;

        //    var metaName = GetMetaDisplayName(property);
        //    if (!string.IsNullOrEmpty(metaName))
        //        return metaName;

        //    return property.Name.ToString();
        //}

        //private string GetAttributeDisplayName(PropertyInfo property)
        //{
        //    var atts = property.GetCustomAttributes(
        //        typeof(DisplayNameAttribute), true);
        //    if (atts.Length == 0)
        //        return null;
        //    return (atts[0] as DisplayNameAttribute).DisplayName;
        //}

        //private string GetMetaDisplayName(PropertyInfo property)
        //{
        //    var atts = property.DeclaringType.GetCustomAttributes(
        //        typeof(MetadataTypeAttribute), true);
        //    if (atts.Length == 0)
        //        return null;

        //    var metaAttr = atts[0] as MetadataTypeAttribute;
        //    var metaProperty =
        //        metaAttr.MetadataClassType.GetProperty(property.Name);
        //    if (metaProperty == null)
        //        return null;
        //    return GetAttributeDisplayName(metaProperty);
        //}
    } 
}
