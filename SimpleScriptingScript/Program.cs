using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        public Program()
        {

            // It's recommended to set RuntimeInfo.UpdateFrequency
            // here, which will allow your script to run itself without a
            // timer block.
            Echo("Starting: SSS/FLIP");

        }

        public void Main(String argument, UpdateType updateSource)
        {

            Char commandSeparator = ';';

            // Trim whitespaces from begin and end to counter IsNullOrEmpty not being called.
            String customData = Me.CustomData.Trim();

            String[] commandList = customData.Split(commandSeparator);

            foreach (var command in commandList)
            {

                if (string.IsNullOrEmpty(command))
                {

                    continue; // Empty strings arent to be processed!

                }

                String[] conditions = getConditions(command);

                foreach (var condition in conditions)
                {

                    String tCondition = condition.Trim(); // @TODO: Find a better way to trim...

                    if (string.IsNullOrEmpty(tCondition))
                    {

                        continue; // Empty strings arent to be processed!

                    }

                    IDictionary<string, string> conditionProperties = getConditionProperties(tCondition);

                    // Make sure the block exists before we retrieve it!
                    bool blockExists = checkBlockByName(conditionProperties["name"]);

                    if (!blockExists)
                    {

                        Echo("Block with name: " + conditionProperties["name"] + " not found, skipping condition: " + tCondition);

                        continue;

                    }

                    IMyTerminalBlock block = getBlockByName(conditionProperties["name"]);

                    // Check if block has required property?

                    bool conditionCheck = checkBlockProperty(conditionProperties, block);

                }

            }

        }

        public bool checkBlockProperty(IDictionary<string, string> conditionProperties, IMyTerminalBlock block)
        {

            Echo(conditionProperties["property"]);


            // var hasProp = block.HasProperty(conditionProperties["property"]);



            // var test = block.GetType().GetProperty(conditionProperties["property"]).GetValue(block, null);
            // Echo(test.ToString());


            List<ITerminalAction> actions = new List<ITerminalAction>();
            block.GetActions(actions);

            foreach (var action in actions)
            {

                // Echo(action.Id + " " + action.Name);

                if (action.Id.ToString() == conditionProperties["property"].ToString())
                {

                    Echo("Found its Id!");

                }

                if (action.Name.ToString() == conditionProperties["property"])
                {

                    Echo("Found its name!");

                }

            }

            // Type type = block.GetType();
            // Echo(type.ToString());
            // PropertyInfo propInfo = type.GetProperty(conditionProperties["property"]);
            // var propInfo = type.GetProperty(conditionProperties["property"]);

            // Echo(block[conditionProperties["property"]]);

            // var hasProp = block.conditionProperties["property"];

            // Echo(propInfo.ToString());
            //
            // if(!propInfo){
            //
            //     Echo("Property ("+ conditionProperties["property"] +") does not exists in: " + conditionProperties["name"]);
            //
            //     return false;
            //
            // }

            return false;

        }

        public IMyTerminalBlock getBlockByName(String blockName)
        {

            IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName(blockName) as IMyTerminalBlock;

            return block;

        }

        public bool checkBlockByName(String blockName)
        {

            IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName(blockName) as IMyTerminalBlock;

            if (block == null)
            {

                return false;

            }
            else
            {

                return true;

            }

        }

        public IDictionary<string, string> getConditionProperties(String condition)
        {

            Char propertySep = '.';
            Char valueSep = '=';

            // Weird splitting magic to parse!
            String[] splitCondition = condition.Split(propertySep);
            String[] keyValue = splitCondition[1].Split(valueSep);

            String blockName = pluckString("[", "]", condition);
            String blockPropery = keyValue[0];
            String blockValue = keyValue[1];

            IDictionary<string, string> block = new Dictionary<string, string>();

            block["name"] = blockName;
            block["property"] = blockPropery;
            block["value"] = blockValue;

            return block;

        }

        public String[] getConditions(String command)
        {

            Char conditionSeparator = ',';

            String extractedConditions = pluckString("if(", ")", command);

            String[] conditions = extractedConditions.Split(conditionSeparator);

            return conditions;

        }

        public String pluckString(String startSep, String endSep, String stringToPluck)
        {

            stringToPluck = stringToPluck.Trim();

            int posFrom = stringToPluck.IndexOf(startSep) + startSep.Length;
            int posTo = stringToPluck.LastIndexOf(endSep);

            String pluckedString = stringToPluck.Substring(posFrom, posTo - posFrom);

            return pluckedString;

        }

    }
}