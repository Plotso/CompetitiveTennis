﻿namespace CompetitiveTennis.Exceptions;

public class InvalidInputDataException : Exception
{
    public InvalidInputDataException(string message) : base(message)
    { }
}