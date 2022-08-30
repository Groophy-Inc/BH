using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using BH.ErrorHandle;
using BH.Parser.Commands.SubCommand;
using BH.Structes;
using BH.Structes.BodyClasses;

namespace BH.Parser.Commands
{
    
    public class com
    {
        public static bool isEnd = false;
        public static int left = 0;

        public static void Init()
        {
            var com = new CommandBuilder();
            com.Create("com",new string[]
            {
                "[new]",
                "[method]",
                "[->]",
                "[$]",
                "[as]",
                "[cf|ps|cs]",
                "[:]",
                "[\":\"]"
            }, Decompose);
            Parse.Commands.Add(com);
        }
        
        public static int Decompose(pbp_Command command)
        {
            Runner.Run(ref command, ref left, ref isEnd);

            
            
            if (isEnd)
            {
                isEnd = false;

                bool isFound = false;
                Element ele = (Element)Varriables.TryGet(command.Commands[3].Result.ToString(), ref isFound).Obj;

                var appcs = ele.appcs;

                var _ = new _Void();

                foreach (var attribute in ((attributes)command.Commands[7].Result).attr)
                {
                    switch (attribute.key.Replace('I', 'i').ToLower())
                    {
                        case "access":
                            _.Access = attribute.value;
                            break;
                        case "args":
                            _.Args = attribute.value.Split(',').ToList();
                            break;
                        case "name":
                            _.Name = attribute.value;
                            break;
                        case "code":
                            _.Code = attribute.value;
                            break;
                        case "isfield":
                            _.isField = Convert.ToBoolean(attribute.value);
                            break;
                        case "returntype":
                            _.ReturnType = attribute.value;
                            break;
                        case "fielddefualt":
                            _.FieldDefualt = attribute.value;
                            break;
                    }
                }
                
                appcs.Classes.First().Voides.Add(_);
                
                ele.appcs = appcs;

                Varriables.AddorUpdate(command.Commands[3].Result.ToString(), ele);

                Parse.EndProcess();
                left = 0;
            }

           
            
            return 0;
        }
    }
}