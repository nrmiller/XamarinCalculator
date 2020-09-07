using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinCalculator.Models;
using XamarinCalculator.Services;

namespace XamarinCalculator.ViewModels
{
    public class CalculatorViewModel: INotifyPropertyChanged
    {
        private readonly IEditorService calculatorService;

        public CalculatorViewModel(IEditorService calculatorService)
        {
            this.calculatorService = calculatorService;

            Input = new Command<Key>(ProcessInput);
            Clear = new Command(() =>
            {
                WorkingValue = string.Empty;
                Result = string.Empty;
                ActiveOperator = null;
                calculatorService.Clear();
                Negate.ChangeCanExecute();
            });

            Add = new Command(() => InvokeOperator(Operator.Add));
            Subtract = new Command(() => InvokeOperator(Operator.Subtract));
            Multiply = new Command(() => InvokeOperator(Operator.Multiply));
            Divide = new Command(() => InvokeOperator(Operator.Divide));
            
            Negate = new Command(() =>
            {
                WorkingValue = (-decimal.Parse(WorkingValue)).ToString();
            }, () =>
            {
                return !string.IsNullOrEmpty(WorkingValue) && WorkingValue.Length < Constants.MaxCharacters;
            });

            Evaluate = new Command(() =>
            {
                if (!string.IsNullOrEmpty(WorkingValue))
                {
                    // Is there an active operator? If so, apply it.
                    if (ActiveOperator != null)
                    {
                        if (!string.IsNullOrEmpty(Result))
                        {
                            try
                            {
                                Result = ApplyOperator(WorkingValue, Result, (Operator)ActiveOperator);
                                if (Result.Length <= Constants.MaxCharacters)
                                {
                                    WorkingValue = string.Empty;
                                    ActiveOperator = null;
                                    calculatorService.Clear();
                                }
                                else if (ShouldTruncate(Result))
                                {
                                    // Truncate when less than 1.
                                    WorkingValue = string.Empty;
                                    Result = Result.Substring(0, Constants.MaxCharacters);
                                    ActiveOperator = null;
                                    calculatorService.Clear();
                                }
                                else
                                {
                                    Result = "Error";
                                    WorkingValue = string.Empty;
                                    ActiveOperator = null;
                                    calculatorService.Clear();
                                }
                            }
                            catch (OverflowException)
                            {
                                Result = "Error";
                                WorkingValue = string.Empty;
                                ActiveOperator = null;
                                calculatorService.Clear();
                            }
                        }
                        else
                        {
                            Result = WorkingValue;
                            WorkingValue = string.Empty;
                            ActiveOperator = null;
                            calculatorService.Clear();
                        }
                    }
                }

                Negate.ChangeCanExecute();
            });
        }

        private void InvokeOperator(Operator op)
        {
            if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(WorkingValue))
            {
                return;
            }

            if (!string.IsNullOrEmpty(WorkingValue))
            {
                // Is there an active operator? If so, apply it.
                if (ActiveOperator != null)
                {
                    if (!string.IsNullOrEmpty(Result))
                    {
                        try
                        {
                            Result = ApplyOperator(WorkingValue, Result, (Operator)ActiveOperator);
                            if (Result.Length <= Constants.MaxCharacters)
                            {
                                WorkingValue = string.Empty;
                                ActiveOperator = op;
                                calculatorService.Clear();
                            }
                            else if (ShouldTruncate(Result))
                            {
                                // Truncate when less than 1.
                                WorkingValue = string.Empty;
                                Result = Result.Substring(0, Constants.MaxCharacters);
                                ActiveOperator = op;
                                calculatorService.Clear();
                            }
                            else
                            {
                                Result = "Error";
                                WorkingValue = string.Empty;
                                ActiveOperator = null;
                                calculatorService.Clear();
                            }
                        }
                        catch (OverflowException)
                        {
                            Result = "Error";
                            WorkingValue = string.Empty;
                            ActiveOperator = null;
                            calculatorService.Clear();
                        }
                    }
                }
                else
                {
                    Result = WorkingValue;
                    WorkingValue = string.Empty;
                    ActiveOperator = op;
                    calculatorService.Clear();
                }
            }
            else
            {
                ActiveOperator = op;
            }

            Negate.ChangeCanExecute();
        }

        private string ApplyOperator(string workingValue, string result, Operator op)
        {
            return op switch
            {
                Operator.Add => (decimal.Parse(result) + decimal.Parse(workingValue)).ToString(),
                Operator.Subtract => (decimal.Parse(result) - decimal.Parse(workingValue)).ToString(),
                Operator.Multiply => (decimal.Parse(result) * decimal.Parse(workingValue)).ToString(),
                Operator.Divide => (decimal.Parse(result) / decimal.Parse(workingValue)).ToString(),
                _ => throw new ArgumentException("Cannot apply a no-op or percent.")
            };
        }

        private async void ProcessInput(Key key)
        {
            WorkingValue = await Task.Run<string>(() => calculatorService.ProcessKey(key));
            Negate.ChangeCanExecute();
        }

        public bool ShouldTruncate(string result)
        {
            if (result.Length <= Constants.MaxCharacters)
            {
                return false;
            }
            else if (!result.Contains("."))
            {
                return false; // No decimal means it should be an "Error".
            }
            else
            {
                // Contains a decimal and > 21 chars.
                int charsBeforeDecimal = result.IndexOf(".");
                if (charsBeforeDecimal > Constants.MaxCharacters)
                {
                    return false;
                }
                else
                {
                    return true; // Number can be truncated.
                }
            }
        }

        public Command Clear { get; }
        public Command Input { get; }
        public Command Add { get; }
        public Command Subtract { get; }
        public Command Multiply { get; }
        public Command Divide { get; }
        public Command Negate { get; }
        public Command Evaluate { get; }

        private string workingValue;
        public string WorkingValue
        {
            get => workingValue;
            set
            {
                if (workingValue != value)
                {
                    workingValue = value;
                    RaisePropertyChanged(nameof(WorkingValue));
                }
            }
        }

        private string result;
        public string Result
        {
            get => result;
            set
            {
                if (result != value)
                {
                    result = value;
                    RaisePropertyChanged(nameof(Result));
                }
            }
        }

        private Operator? activeOperator;
        public Operator? ActiveOperator
        {
            get => activeOperator;
            set
            {
                if (activeOperator != value)
                {
                    activeOperator = value;
                    RaisePropertyChanged(nameof(ActiveOperator));
                }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
