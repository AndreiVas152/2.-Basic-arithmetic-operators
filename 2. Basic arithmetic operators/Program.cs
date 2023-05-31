using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Basic_arithmetic_operators
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            //Asks the user to input an equation

            Console.WriteLine("Please input the equation you'd like solved");
                      

            //Stores the user input in a variable, removes spaces and replaces periods with commas to get around localization issues

            var equation = Console.ReadLine().ToString();
            equation = equation.Replace(" ", "");
            equation = equation.Replace(".", ",");
            
            //Initializes a new Operation method and sets the starting point as the left side of the equation

            var leftSide = true;
            var operation = new Operation();

            //Runs through each character in the string provided and converts to readable values
            
            for (int i = 0; i < equation.Length; i++)
            {
                try
                {  
                    //Checks if the character is part of a number

                    if ("0123456789.,".Any(c => equation[i] == c))
                    {
                        //Checks which side of the equation we're on and adds a character to that number
                        if (leftSide)
                            operation.LeftSide = AddNumberPart(operation.LeftSide, equation[i]);

                        else
                            operation.RightSide = AddNumberPart(operation.RightSide, equation[i]);
                    }
                    //Checks if the character is an operator

                    else if ("+-*/".Any(c => equation[i] == c))
                    {
                        //Checks which side of the operation we're on

                        if (!leftSide)
                        {
                            //Sets the operator type if on the right side and resolves the equation up to this point

                            var operatorType = GetOperationType(equation[i]);
                            //Checks whether current operator is part of a negative number after a different operator

                            if (operatorType == OperationType.minus && "/*-+".Any(c => equation[i - 1] == c))
                            {
                                operation.RightSide = AddNumberPart(operation.RightSide, equation[i]);

                            }

                            else
                            {
                                operation.LeftSide = CalculateOperation(operation);

                                operation.OperationType = operatorType;

                                operation.RightSide = string.Empty;
                            }
                        }
                        
                        else
                        {
                            var operatorType = GetOperationType(equation[i]);

                            //Checks if a left side exists or if it starts with a negative number

                            if (operation.LeftSide.Length == 0)
                            {
                                if (operatorType != OperationType.minus)
                                    throw new InvalidOperationException("First character cannot be an operator outside of defining a negative number");

                                operation.LeftSide += equation[i];
                            }
                            else
                            {
                                operation.OperationType = operatorType;
                                leftSide = false;
                            }
                        }
                    }

                    else if ("(".Any(c=> equation[i] == c))
                    {
                        //Saves the values present before parentheses and restores all variables to default before resolving parentheses

                        operation.SavedValue = operation.LeftSide;
                        operation.SavedOperation = operation.OperationType;
                        operation.RightSide = string.Empty;
                        operation.LeftSide = string.Empty;
                        operation.OperationType = default;
                        leftSide = true;

                        while (")".Any(c=> equation[i] != c))
                        {
                            i++;
                            if ("0123456789.,".Any(c => equation[i] == c))
                            {
                                //Checks which side of the equation we're on and adds a character to that number
                                if (leftSide)
                                    operation.LeftSide = AddNumberPart(operation.LeftSide, equation[i]);

                                else
                                    operation.RightSide = AddNumberPart(operation.RightSide, equation[i]);
                            }
                            //Checks if the character is an operator

                            else if ("+-*/".Any(c => equation[i] == c))
                            {
                                //Checks which side of the operation we're on

                                if (!leftSide)
                                {
                                    //Sets the operator type if on the right side and resolves the equation up to this point

                                    var operatorType = GetOperationType(equation[i]);
                                    //Checks whether current operator is part of a negative number after a different operator

                                    if (operatorType == OperationType.minus && "/*-+".Any(c => equation[i - 1] == c))
                                    {
                                        operation.RightSide = AddNumberPart(operation.RightSide, equation[i]);

                                    }

                                    else
                                    {
                                        operation.LeftSide = CalculateOperation(operation);

                                        operation.OperationType = operatorType;

                                        operation.RightSide = string.Empty;
                                    }
                                }

                                else
                                {
                                    var operatorType = GetOperationType(equation[i]);

                                    //Checks if a left side exists or if it starts with a negative number

                                    if (operation.LeftSide.Length == 0)
                                    {
                                        if (operatorType != OperationType.minus)
                                            throw new InvalidOperationException("First character cannot be an operator outside of defining a negative number");

                                        operation.LeftSide += equation[i];
                                    }
                                    else
                                    {
                                        operation.OperationType = operatorType;
                                        leftSide = false;
                                    }
                                }
                            }

                        }
                        //Calculates the final operation in parentheses and recalls the leftside value before parentheses 
                        operation.LeftSide = CalculateOperation(operation);
                        operation.RightSide = operation.LeftSide;
                        operation.LeftSide = operation.SavedValue;
                        operation.OperationType = operation.SavedOperation;                        
                    }
                }
                catch (Exception) { Console.WriteLine("Invalid equation"); }                               
            }

            //Resolves the final calculation once the string has been fully parsed and outputs the solution to the user
            
            operation.LeftSide = CalculateOperation(operation);
            Console.WriteLine($"-----------");
            Console.WriteLine($"{operation.LeftSide}");
            Console.ReadLine();


            //Calculates an operation by parsing the left and right side of the operator
            //then executing the desired mathematical operation

            string CalculateOperation(Operation op)

            {
                double left = 0;
                double right = 0;

                double.TryParse(operation.LeftSide, out left);
                double.TryParse(operation.RightSide, out right);
                
                try
                {
                    switch (operation.OperationType)
                    {
                        case OperationType.plus:
                            return (left + right).ToString();
                        case OperationType.minus:
                            return (left - right).ToString();
                        case OperationType.multiply:
                            return (left * right).ToString();
                        case OperationType.divide:
                            return (left / right).ToString();
                        default:
                            throw new InvalidOperationException("Unknown operator type when calculating equation");

                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException();
                }
            }
            //Adds a character to the number
            //Checks for existing periods and throws an exception if multiple

            string AddNumberPart(string currentNumber, char newChar)
            {
                if (newChar == '.' && currentNumber.Contains('.'))
                    throw new InvalidOperationException($"Number {currentNumber} contains multiple periods");

                return currentNumber + newChar;
            }

            //Checks operator character and sets it

            OperationType GetOperationType(char character)
            {
                switch (character)
                {
                    case '+':
                        return OperationType.plus;
                    case '-':
                        return OperationType.minus;
                    case '*':
                        return OperationType.multiply;
                    case '/':
                        return OperationType.divide;
                    default:
                        throw new InvalidOperationException($"{character} is not a known operator");
                }
            }
        }
    }
}
