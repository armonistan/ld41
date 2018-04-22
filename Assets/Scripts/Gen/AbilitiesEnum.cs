using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;
using UnityEngine;

namespace PlayerAbilities
{
    public enum Abilities
    {
        [StringValue("Free Agent"), Description("Doesn't cost anything to recruit, but you get what you pay for.")]
        FreeAgent,
        [StringValue("I Did Not Hit Her"), Description("I did not. Break out of tackles 20% easier.")]
        DidNotHit,
        [StringValue("Stylin'"), Description("Has an extra style point. If I can believe in the dinosaurs, then they can believe in me. I think about that and it makes me happy. Then, I bust out.")]
        Stylin,
        [StringValue("Absolute Unit"), Description("Look at the size of this lad. Nothing can topple him. Break out of tackles 60% easier.")]
        Unit,
        [StringValue("Stiffie"), Description("Harder than a boiled egg. A hardboiled egg. Really hard. Like you wouldn't believe. Stiff arms are 30% stronger.")]
        Stiffie,
    }

    public class StringValue : Attribute
    {
        private readonly string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public class AbilitiesEnum {
        public static Abilities getRandom()
        {
            Array abilitiesArray = Abilities.GetValues(typeof(Abilities));
            return (Abilities) abilitiesArray.GetValue((int) (UnityEngine.Random.value * abilitiesArray.Length));
        }

        public static string GetAttributeDescription(object value)
        {
            string retVal = string.Empty;
            try
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                   (DescriptionAttribute[])fieldInfo.GetCustomAttributes
                   (typeof(DescriptionAttribute), false);

                retVal = ((attributes.Length > 0) ? attributes[0].Description : value.ToString());
            }
            catch (NullReferenceException)
            {
                //Occurs when we attempt to get description of an enum value that does not exist
            }
            finally
            {
                if (string.IsNullOrEmpty(retVal))
                    retVal = "This ability is an enigma to all.";
            }

            return retVal;
        }

        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}
