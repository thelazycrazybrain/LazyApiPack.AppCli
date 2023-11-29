# About this pack
This library enables you to map console commands to functions.

# Console command format
The format is the same as in the windows console.

# Sample application and bootstrap
To create an application, simply create a class that inherits from `ConsoleApplication`.

```csharp
public class ConsoleApp1 : ConsoleApplication
{
   
    [ConsoleFunction]
    public string HelloWorld(string message)
    {
        Print("Hello ");
        Print(message);
        PrintLine();
    }
}

```

To use it, check for the IsRunning property. If true, you can pass console lines to the application.
The return object is the result of the function or `NULL` (User defined).

```csharp
private static void Main(string[] args)
{
    var app = new ConsoleApp1();
    while (app.IsRunning)
    {
        var @in = Console.ReadLine();
        if (@in == null)
        {
            continue;
        }
        app.Execute(@in);
    }
}
```

The function can be called with
```console
HelloWorld -message "You beautiful world"
```

## ConsoleFunction and ConsoleFunctionParameter
To mark a function as a console function, it has to be public and decorated with the ConsoleFunction attribute.
A parameter can be decorated with the ConsoleFunctionParameter.
You can specify, whether the command should be case sensitive or specific aliases for the function.
- Note, that spaces are not allowed due to the console syntax rules!


# Map console command to function
To use a simple mapping, decorate the method and the parameters with the `ConsoleFunction` and `ConsoleFunctionParameter` attributes.

```csharp
[ConsoleFunction]
public string Format(
        [FunctionParameter]
        string text,

        [FunctionParameter(DefaultValue = false)]
        bool upperCase,

        [FunctionParameter]
        string arg)
{
    var format = string.Format(text, arg);
    return upperCase ? format.ToUpper() : format;
}
```
This method can be called with

```console
Format -text "This is a formatted {0} text." -arg "and replaced" -upperCase
```

# Map console command to function with simple parameters
To use a method like the `main(string[] args)` function, write a method.
`public int MyMain(params string[] args)` and use it in the console like
`MyMain -myparam test -myparam2 "This is also a test"
The function gets called with a list of parameters
- -myparam
- test
- -myparam2
- "This is also a test"

# Print, PrintLine
To use print ability, override the `OnPrint` function and write the appropriate logic to print on screen. Note that new lines are automatically appended when used with `PrintLine()`.
The default behaivor without overloading the method prints automatically to `Console`.

# Errors
If a function is not known to the application, the method `OnFunctionNotFound(FunctionNotFoundEventArgs e)` is called. If you overload it, you can set the `e.ContinueWithoutError = true` to continue the application. If you don't overload it or don't set the flag, the application throws a `FunctionNotFoundException` at `Execute`.

If the command is malformed, the method `OnInvalidCommand` is called. If you overload it, you can set the `e.ContinueWithoutError = true` to continue the application. If you don't overload it or don't set the flag, the application throws a `InvalidCommandException` at `Execute`.
```csharp
protected override void OnFunctionNotFound(FunctionNotFoundEventArgs e)
{
    PrintLine($"Command {e.FunctionName} not found.");
    e.ContinueWithoutError = true;
}

protected override void OnInvalidCommand(InvalidCommandEventArgs e)
{
    PrintLine($"Command '{e.Command}' is invalid.");
    e.ContinueWithoutError = true;
}
```


# Documentation
To document functions and make it available via the `man` command (see #Man), use the `Documentation` attribute for the function and its parameters.

```csharp
[ConsoleFunction]
[Documentation("Prints Hello world and your custom message")
public string HelloWorld([Documentation("Prints this message to the screen")]string message)
{
    Print("Hello ");
    Print(message);
    PrintLine();
}
```
# Built in functions
## Exit
Closes the application


## Man
Write
```console
Man FunctionName
```
to get the documentation of the function.