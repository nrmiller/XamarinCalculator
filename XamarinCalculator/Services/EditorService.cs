using System;
using XamarinCalculator.Models;

namespace XamarinCalculator.Services
{
    public interface IEditorService
    {
        public string ProcessKey(Key key);
        public void Clear();
    }

    public class EditorService: IEditorService
    {
        public EditorService()
        {
        }

        public string Input { get; set; } = string.Empty;

        public string ProcessKey(Key key)
        {
            if (key == Key.Backspace)
            {
                Input = (Input.Length > 0) ? Input.Substring(0, Input.Length - 1) : string.Empty;
                return Input;
            }

            if (Input.Contains("."))
            {
                if (key != Key.Decimal)
                {
                    Input += key switch
                    {
                        Key.Num0 => "0",
                        Key.Num1 => "1",
                        Key.Num2 => "2",
                        Key.Num3 => "3",
                        Key.Num4 => "4",
                        Key.Num5 => "5",
                        Key.Num6 => "6",
                        Key.Num7 => "7",
                        Key.Num8 => "8",
                        Key.Num9 => "9",
                        _ => throw new ArgumentException()
                    };
                }
            }
            else
            {
                if (Input == "0")
                {
                    if (key == Key.Num0)
                    {
                        Input = "0";
                    }
                    else if (key == Key.Decimal)
                    {
                        Input = "0.";
                    }
                    else
                    {
                        Input = key switch
                        {
                            Key.Num1 => "1",
                            Key.Num2 => "2",
                            Key.Num3 => "3",
                            Key.Num4 => "4",
                            Key.Num5 => "5",
                            Key.Num6 => "6",
                            Key.Num7 => "7",
                            Key.Num8 => "8",
                            Key.Num9 => "9",
                            _ => throw new ArgumentException()
                        };
                    }
                }
                else if (Input == string.Empty)
                {
                    if (key == Key.Num0)
                    {
                        Input = "0";
                    }
                    else if (key == Key.Decimal)
                    {
                        Input = "0.";
                    }
                    else
                    {
                        Input = key switch
                        {
                            Key.Num1 => "1",
                            Key.Num2 => "2",
                            Key.Num3 => "3",
                            Key.Num4 => "4",
                            Key.Num5 => "5",
                            Key.Num6 => "6",
                            Key.Num7 => "7",
                            Key.Num8 => "8",
                            Key.Num9 => "9",
                            _ => throw new ArgumentException()
                        };
                    }
                }
                else
                {
                    Input += key switch
                    {
                        Key.Num0 => "0",
                        Key.Num1 => "1",
                        Key.Num2 => "2",
                        Key.Num3 => "3",
                        Key.Num4 => "4",
                        Key.Num5 => "5",
                        Key.Num6 => "6",
                        Key.Num7 => "7",
                        Key.Num8 => "8",
                        Key.Num9 => "9",
                        Key.Decimal => ".",
                        _ => throw new ArgumentException()
                    };
                }
            }

            Input = (Input.Length < 21) ? Input : Input.Substring(0, Constants.MaxCharacters);
            return Input;
        }

        public string Evaluate()
        {
            return "";
        }

        public void Clear()
        {
            Input = string.Empty;
        }
    }
}
