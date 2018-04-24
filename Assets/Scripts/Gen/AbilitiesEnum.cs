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
        [StringValue("Free Agent"), Description("Doesn't cost anything to recruit, but you get what you pay for."), CostMul("1.0")]
        FreeAgent,
        [StringValue("I Did Not Hit Her"), Description("I did not. And she did not hit me. Nothing hit me. Nothing can ever hit me. Spins have 50% chance to not use style points."), CostMul("1.1")]
        DidNotHit,
        [StringValue("Stylin'"), Description("Style points regenerate 2x faster. Cannot run backwards. If I can believe in the dinosaurs, then they can believe in me. I think about that and it makes me happy. Then, I bust out."), CostMul("1.4")]
        Stylin,
        [StringValue("Absolute Unit"), Description("Look at the size of this lad. Regenerates Health over time, but you cannot spin."), CostMul("1.8")]
        Unit,
        [StringValue("Stiffie"), Description("Harder than a boiled egg. A hardboiled egg. Stiff arms can hold for twice as long."), CostMul("1.1")]
        Stiffie,
        [StringValue("Shephard"), Description("I lead the chaerge. Tackles last twice as long, but you're half as quick."), CostMul("1.4")]
        Shephard,
        [StringValue("Greased Lightnin'"), Description("I'm on ice! Like a speeding bullet! can't catch me! You're twice as fast, but have half as much dexterity."), CostMul("1.1")]
        GreasedLightning,
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

    public class CostMul : Attribute
    {
        private readonly string _value;

        public CostMul(string value)
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
            return (Abilities) abilitiesArray.GetValue((int)(UnityEngine.Random.value * (abilitiesArray.Length - 1)) + 1);
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

        public static float GetCostMultiplier(Enum value)
        {
            float output = 1f;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            CostMul[] attrs =
               fi.GetCustomAttributes(typeof(CostMul),
                                       false) as CostMul[];
            if (attrs.Length > 0)
            {
                output = float.Parse(attrs[0].Value);
            }

            return output;
        }
    }
}
