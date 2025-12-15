public abstract class RuntimeObject
{
	public abstract double AsNumber();
	public abstract string AsString();
	public abstract bool AsBool();
}
public abstract class RuntimeObject<T> : RuntimeObject
{
	public T Value => _value;
	protected T _value;

	public virtual void SetValue(T val)
	{
		_value = val;
	}

	public virtual void SetValue(object? val)
	{
		throw new Exception("type error, can't convert val to type");
	}
}

public class LJString : RuntimeObject<string>
{
	public LJString(string val)
	{
		_value = val;
	}

	public override double AsNumber()
	{
		throw new Exception("Cannot implicitly convert string to number");
	}

	public override bool AsBool()
	{
		throw new Exception("cannot implicitly convert string to bool");
	}

	public override string AsString()
	{
		return _value;
	}
}

public class Identifier : LJString
{
	public Identifier(string val) : base(val)
	{ }
}
public class LJNumber : RuntimeObject<double>
{
	public LJNumber(double val)
	{
		_value = val;
	}

	public override double AsNumber()
	{
		return _value;
	}

	public override string AsString()
	{
		return _value.ToString();
	}

	public override bool AsBool()
	{
		return _value == 0;
	}
}

public class LJPoint : RuntimeObject<(double X, double Y)>
{
	public override double AsNumber()
	{
		throw new Exception("cannot implicitly convert point to number");
	}

	public override string AsString()
	{
		throw new Exception("cannot implicitly convert point to string");
	}

	public override bool AsBool()
	{
		throw new Exception("cannot implicitly convert point to bool");
	}
}