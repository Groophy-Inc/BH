using System;
using System.Collections.Generic;
using BH.ErrorHandle;
using BH.Script.Types;

namespace BH
{
    public class Varriables
    {
        public static Dictionary<string, IVarriable> Vars = new Dictionary<string, IVarriable>();

        public static string FixContent(string content)
        {
            string[] keys = content.Split('$');

            bool refuse = false;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i == 0)
                {
                    if (keys[i].EndsWith('\\'))
                    {
                        refuse = true;
                        keys[i] = keys[i] + "$";
                    }
                }
                else
                {
                    if (refuse) { refuse = false; continue; }

                    bool hasCon = false;
                    string suffix = "";
                    string repName = FindKey(keys[i].Split(' ')[0], ref hasCon, ref suffix);
                    var ClosestVar = Varriables.TryGet(repName);
                    string[] suffixs = suffix.Split('.');
                    object lastObject = (hasCon)
                        ? CF.GetFieldOfObject(ClosestVar.Obj, suffixs[0])
                        : ClosestVar.Obj;
                    for (int j = 1; j < suffixs.Length; j++)
                    {
                        lastObject = lastObject.GetType().GetProperty(suffixs[i]).GetValue(lastObject, null);
                    }
                    string repValue = lastObject.ToString();
                    if (hasCon)
                    {
                        repName += "!" + suffix + "!";
                    }
                    keys[i] = keys[i].Replace(repName, repValue);

                    if (keys[i].EndsWith('\\'))
                    {
                        refuse = true;
                        keys[i] = keys[i] +"$";
                    }
                }
            }
            string retenv = "";
            for (int i = 0; i < keys.Length; i++) retenv += keys[i].Replace("\\$", "$");
            
            Logs.Log($"ContentFix\r\nContent: {content}\r\nFixedContent: {retenv}");
            return retenv;
        }

        private static string FindKey(string fullKey, ref bool hasCon, ref string Suffix)
        {
            hasCon = false;
            for (int i = 1; i < fullKey.Length; i++)
            {
                if (JustTryGet(fullKey.Substring(0,i)))
                {
                    if (fullKey.Length > i + 1)
                    {
                        if (fullKey.Substring(i, 1) == "!")
                        {
                            hasCon = true;
                            for (int j = i + 1; j < fullKey.Length; j++)
                            {
                                if (fullKey[j] == '!') break;
                                Suffix += fullKey[j];
                            }
                        }
                    }
                    return fullKey.Substring(0,i);
                }
            }
            return fullKey;
        }

        public static bool JustTryGet(string key)
        {
            if (key.ToLower() == "thingno") return false;
            try
            {
                var x = Vars[key];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IVarriable TryGet(string key)
        {
            if (key.ToLower() == "thingno") return null;
            try
            {
                return Vars[key];
            }
            catch
            {
                return null;
            }
        }

        public static bool AddorUpdate(string key, string value)
        {
            if (key.ToLower() == "thingno") return false;
            try
            {
                var val = new IVarriable()
                {
                    Obj = value,
                    Packet_Type = "Null"
                };
                if (!Vars.TryAdd(key, val))
                {
                    val = Vars[key];

                    Vars[key] = new IVarriable()
                    {
                        Obj = value,
                        Packet_Type = val.Packet_Type
                    };
                }
                return false;
            }
            catch { return false; }
        }

        public static bool AddorUpdate(string key, object value)
        {
            if (key.ToLower() == "thingno") return false;
            try
            {
                var val = new IVarriable()
            {
                Obj = value,
                Packet_Type = "Null"
            };
            if (!Vars.TryAdd(key, val))
            {
                val = Vars[key];

                Vars[key] = new IVarriable()
                {
                    Obj = value,
                    Packet_Type = val.Packet_Type
                };
            }
                return true;
            }
            catch { return false; }
        }

        public static bool AddorUpdate(string key, string value, string packet_Type)
        {
            if (key.ToLower() == "thingno") return false;
            try
            {
                var val = new IVarriable()
            {
                Obj = value,
                Packet_Type = packet_Type
            };
            if (!Vars.TryAdd(key, val))
            {
                Vars[key] = new IVarriable()
                {
                    Obj = value,
                    Packet_Type = packet_Type
                };
            }
                return true;
            }
            catch { return false; }
        }
        public static bool AddorUpdate(string key, object value, string packet_Type)
        {
            if (key.ToLower() == "thingno") return false;
            try
            {
                var val = new IVarriable()
            {
                Obj = value,
                Packet_Type = packet_Type
            };
            if (!Vars.TryAdd(key, val))
            {
                Vars[key] = new IVarriable()
                {
                    Obj = value,
                    Packet_Type = packet_Type
                };
            }
                return true;
            }
            catch { return false; }

        }

        public static bool UpdatePacketType(string key, string packet_Type)
        {
            if (key.ToLower() == "thingno") return false;
            try
            {
                var val = Vars[key];
                Vars[key] = new IVarriable()
                {
                    Obj = val.Obj,
                    Packet_Type = packet_Type
                };
                return true;
            }
            catch { return false; }
        }
    }

    public class IVarriable
    {
        public string Packet_Type { get; set; }
        public object Obj { get; set; }
    }
}
