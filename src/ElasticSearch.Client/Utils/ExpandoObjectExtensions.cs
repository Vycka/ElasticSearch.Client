﻿using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Client.Utils
{
    public static class ExpandoObjectExtensions
    {
        public static void AddIfNotNull(this ExpandoObject expandoObject, string key, object value)
        {
            if (value != null)
            {
                IDictionary<string, object> expandoObjectDictionary = expandoObject;

                expandoObjectDictionary.Add(key, value);
            }
        }

        public static IDictionary<string, object> GetItems(this ExpandoObject expandoObject)
        {
            return expandoObject;
        }

        public static void Add(this ExpandoObject expandoObject, string key, object value)
        {
            IDictionary<string, object> expandoObjectDictionary = expandoObject;
            expandoObjectDictionary.Add(key, value);
        }

        public static void AddOrUpdate(this ExpandoObject expandoObject, string key, object value)
        {
            IDictionary<string, object> expandoObjectDictionary = expandoObject;


            if (expandoObjectDictionary.ContainsKey(key))
                expandoObjectDictionary[key] = value;
            else
                expandoObjectDictionary.Add(key, value);
        }
    }
}