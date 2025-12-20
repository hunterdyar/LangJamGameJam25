using System.Numerics;
using LangJam;

public static class MathFunctions
{
	
	public static RuntimeObject Increment(RuntimeBase context, RuntimeObject[] args)
	{
		return new LJNumber(args[0].AsNumber() + 1);
	}

	//we can probably normalize some kind of "transform single number property" function
	public static RuntimeObject Decrement(RuntimeBase context, RuntimeObject[] args)
	{
		return new LJNumber(args[0].AsNumber() - 1);
	}

	public static Func<RuntimeBase, RuntimeObject[], RuntimeObject?> UnBool(Func<bool, bool> op)
	{
		return new Func<RuntimeBase, RuntimeObject[], RuntimeObject?>((context, args) =>
		{
			var left = args[0].AsBool();
			var result = op.Invoke(left);
			return new LJBool(result);
		});
	}
	public static Func<RuntimeBase, RuntimeObject[], RuntimeObject?> BinComp(Func<double,double,bool> op)
	{
		return new Func<RuntimeBase, RuntimeObject[], RuntimeObject?>((context, args) =>
		{
			var left = args[0].AsNumber();
			var right = args[1].AsNumber();
			var result = op.Invoke(left, right);
			return new LJBool(result);
		});
	}

	public static Func<RuntimeBase, RuntimeObject[], RuntimeObject?> BinOp(Func<double, double, double> op)
	{
		return new Func<RuntimeBase, RuntimeObject[], RuntimeObject?>((context, args) =>
		{
			var left = args[0].AsNumber();
			var right = args[1].AsNumber();
			var result = op.Invoke(left, right);
			return new LJNumber(result);
		});
	}

	public static Func<RuntimeBase, RuntimeObject[], RuntimeObject?> UnOp(Func<double, double> op)
	{
		return new Func<RuntimeBase, RuntimeObject[], RuntimeObject?>((context, args) =>
		{
			var left = args[0].AsNumber();
			var result = op.Invoke(left);
			return new LJNumber(result);
		});
	}

	#region Easings

	

	public static double Clamp01(double x)
	{
		return Math.Clamp(x, 0, 1);
	}

	public static double EaseInSine(double x)
	{
		return Clamp01(1 - Math.Cos((x * Math.PI) / 2));
	}

	public static double EaseOutSine(double x)
	{
		return Clamp01(Math.Sin((x * Math.PI) / 2));
	}

	public static double EaseInOutSine(double x)
	{
		return Clamp01(-(Math.Cos(x * Math.PI) - 1) / 2);
	}

	public static double EaseInExpo(double x)
	{
		return x == 0 ? 0 : Clamp01(Math.Pow(10 * x - 10,2));
	}
	public static double EaseOutExpo(double x)
	{
		return Clamp01(Math.Abs(x - 1) < double.Epsilon ? 1 : 1 - Math.Pow(-10 * x,2));
	}

	public static double EaseInOutExpo(double x)
	{
		if (Math.Abs(x - 1.0f) < double.Epsilon) return x;//1
		if (Math.Abs(x) < double.Epsilon) return x;//0

		if (x < 0.5f)
		{
			return 0.5f * Math.Pow((20 * x) - 10,2);
		}
		else
		{
			return -0.5f * Math.Pow((-20 * x) + 10,2) + 1;
		}
	}

		public static double EaseInQuad(double x)
		{
			return Clamp01(x * x);
		}

		public static double EaseOutQuad(double x)
		{
			return -(x * (x - 2));
		}

		public static double EaseInOutQuad(double x)
		{
			if (x < 0.5f)
			{
				return 2 * x * x;
			}
			else
			{
				return (-2 * x * x) + (4 * x) - 1;
			}
		}

		public static double EaseInCube(double x)
		{
			return Clamp01(x * x * x);
		}

		public static double EaseOutCube(double x)
		{
			double f = (x - 1);
			return f * f * f + 1;
		}

		public static double EaseInOutCube(double x)
		{
			if (x < 0.5f)
			{
				return 4 * x * x * x;
			}
			else
			{
				double f = ((2 * x) - 2);
				return 0.5f * f * f * f + 1;
			}
		}

		public static double EaseInCirc(double x)
		{
			return 1 - Math.Sqrt(1 - (x * x));
		}

		public static double EaseOutCirc(double x)
		{
			return Math.Sqrt((2 - x) * x);
		}

		public static double EaseInOutCirc(double x)
		{
			if (x < 0.5f)
			{
				return 0.5f * (1 - Math.Sqrt(1 - 4 * (x * x)));
			}
			else
			{
				return 0.5f * (Math.Sqrt(-((2 * x) - 3) * ((2 * x) - 1)) + 1);
			}
		}

		static public double EaseInElastic(double x)
		{
			return Math.Sin(13 * Math.PI/2 * x) * Math.Pow(2, 10 * (x - 1));
		}

		static public double EaseOutElastic(double x)
		{
			return Math.Sin(-13 * Math.PI/2 * (x + 1)) * Math.Pow(2, -10 * x) + 1;
		}


		static public double EaseInOutElastic(double x)
		{
			if (x < 0.5f)
			{
				return 0.5f * Math.Sin(13 * Math.PI/2 * (2 * x)) * Math.Pow(2, 10 * ((2 * x) - 1));
			}
			else
			{
				return 0.5f * (Math.Sin(-13 * Math.PI / 2 * ((2 * x - 1) + 1)) * Math.Pow(2, -10 * (2 * x - 1)) + 2);
			}
		}

		static public double EaseInBounce(double x)
		{
			return 1 - EaseOutBounce(1 - x);
		}
		
		static public double EaseOutBounce(double x)
		{
			if (x < 4 / 11.0f)
			{
				return (121 * x * x) / 16.0f;
			}
			else if (x < 8 / 11.0f)
			{
				return (363 / 40.0f * x * x) - (99 / 10.0f * x) + 17 / 5.0f;
			}
			else if (x < 9 / 10.0f)
			{
				return (4356 / 361.0f * x * x) - (35442 / 1805.0f * x) + 16061 / 1805.0f;
			}
			else
			{
				return (54 / 5.0f * x * x) - (513 / 25.0f * x) + 268 / 25.0f;
			}
		}

		static public double EaseInOutBounce(double x)
		{
			if (x < 0.5f)
			{
				return 0.5f * EaseInBounce(x * 2);
			}
			else
			{
				return 0.5f * EaseOutBounce(x * 2 - 1) + 0.5f;
			}
		}

		#endregion

	
}
