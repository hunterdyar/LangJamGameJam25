using System.Runtime;
using LangJam;

public abstract class RuntimeObject
{
	public abstract double AsNumber();
	public abstract string AsString();
	public abstract bool AsBool();

	public virtual LJList AsList()
	{
		throw new Exception("unable to convert object to list/range");
	}
}
public abstract class RuntimeObject<T> : RuntimeObject, IEquatable<RuntimeObject<T>>
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

	#region GeneratedEquality
	
	public bool Equals(RuntimeObject<T>? other)
	{
		if (other is null) return false;
		if (ReferenceEquals(this, other)) return true;
		return EqualityComparer<T>.Default.Equals(_value, other._value);
	}

	public override bool Equals(object? obj)
	{
		return ReferenceEquals(this, obj) || obj is RuntimeObject<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		return EqualityComparer<T>.Default.GetHashCode(_value);
	}

	public static bool operator ==(RuntimeObject<T>? left, RuntimeObject<T>? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(RuntimeObject<T>? left, RuntimeObject<T>? right)
	{
		return !Equals(left, right);
	}

	#endregion

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

public class LJKey : LJString
{
	public LJKey(string val) : base(val)
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

public class LJBool: RuntimeObject<bool>
{
	public LJBool(bool val)
	{
		_value = val;
	}
	public override double AsNumber()
	{
		return _value ? 1 : 0;
	}

	public override string AsString()
	{
		return _value ? "true" : "false";
	}

	public override bool AsBool()
	{
		return _value;
	}
}

public class LJPoint : RuntimeObject<(double X, double Y)>
{
	public LJNumber X
	{
		get => new(_value.X);
		set => _value.X = value.Value;
	} 
	public LJNumber Y
	{
		get => new(_value.Y);
		set => _value.Y = value.Value;
	} 

	public LJPoint(double x, double y)
	{
		_value = (x, y);
	}

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

public class LJSprite : RuntimeObject<Raylib_cs.RenderTexture2D>
{
	public override double AsNumber()
	{
		throw new NotImplementedException();
	}

	public override string AsString()
	{
		throw new NotImplementedException();
	}

	public override bool AsBool()
	{
		throw new NotImplementedException();
	}
}
public class LJRuntimeBaseReference : RuntimeObject<RuntimeBase>{
	public LJRuntimeBaseReference(RuntimeBase rb)
	{
		_value = rb;
	}
	public override double AsNumber()
	{
		throw new NotImplementedException();
	}

	public override string AsString()
	{
		throw new NotImplementedException();
	}

	public override bool AsBool()
	{
		throw new NotImplementedException();
	}
}
public class LJSceneReference : LJRuntimeBaseReference
{
	public LJSceneReference(Scene entity) : base(entity)
	{
		
	}

	public override double AsNumber()
	{
		throw new NotImplementedException();
	}

	public override string AsString()
	{
		throw new NotImplementedException();
	}

	public override bool AsBool()
	{
		throw new NotImplementedException();
	}

	public override LJList AsList()
	{
		throw new Exception("todo: iterate through children");
	}
}

public class LJComponentReference : LJRuntimeBaseReference
{
	private ComponentBase _componentBase;
	public LJComponentReference(ComponentBase componentBase) : base(componentBase)
	{
	}
}

public class LJList : RuntimeObject<List<RuntimeObject>>
{
	public LJList()
	{
		_value = new List<RuntimeObject>();
	}

	public LJList(List<RuntimeObject> list)
	{
		_value = list;
	}
	public override double AsNumber()
	{
		throw new NotImplementedException();
	}

	public override string AsString()
	{
		throw new NotImplementedException();
	}

	public override bool AsBool()
	{
		return _value.Count == 0;
	}

	public override LJList AsList()
	{
		return this;
	}
}