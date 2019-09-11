﻿using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
    [ComponentHandler(typeof(BoolSetting))]
    public class BoolSettingHandler : TypeHandler
    {
        public override Dictionary<string, string[]> Props => new Dictionary<string, string[]>()
        {
            { "text", new[]{ "text" } },
            { "onChange", new[]{ "on-change"} },
            { "value", new[]{ "value"} },
            { "initialValue", new[]{ "initial-value"} },
            { "setEvent", new[]{ "set-event"} },
            { "getEvent", new[]{ "get-event"} },
            { "applyOnChange", new[] { "apply-on-change" } }
        };

        public override void HandleType(Component obj, Dictionary<string, string> data, BSMLParserParams parserParams)
        {
            BoolSetting boolSetting = obj as BoolSetting;

            if (data.TryGetValue("text", out string text))
                boolSetting.LabelText = text;

            if (data.TryGetValue("applyOnChange", out string applyOnChange))
                boolSetting.updateOnChange = Parse.Bool(applyOnChange);

            if (data.TryGetValue("initialValue", out string initialValue))
                boolSetting.Value = Parse.Bool(initialValue);

            if (data.TryGetValue("onChange", out string onChange))
            {
                if (!parserParams.actions.TryGetValue(onChange, out BSMLAction onChangeAction))
                    throw new Exception("on-change action '" + onChange + "' not found");

                boolSetting.onChange = onChangeAction;
            }

            if (data.TryGetValue("value", out string value))
            {
                if (!parserParams.values.TryGetValue(value, out BSMLValue associatedValue))
                    throw new Exception("value '" + value + "' not found");

                boolSetting.associatedValue = associatedValue;
            }

            parserParams.AddEvent(data.TryGetValue("setEvent", out string setEvent) ? setEvent : "apply", boolSetting.ApplyValue);
            parserParams.AddEvent(data.TryGetValue("getEvent", out string getEvent) ? getEvent : "cancel", boolSetting.ReceiveValue);

            boolSetting.Setup();
        }
    }
}
