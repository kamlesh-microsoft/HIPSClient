using System;


namespace HIPSClient.Common.Tools.Attributes
{
  [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
  public sealed class EnumLiteralAttribute : Attribute
  {
    readonly string literal;

    // This is a positional argument
    public EnumLiteralAttribute(string literal)
    {
      this.literal = literal;
    }

    public string Literal
    {
      get { return literal; }
    }
  }
}
