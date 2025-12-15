using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using LangJam.Loader.AST;

namespace LangJam;

public class Entity : RuntimeBase
{
	public LJPoint Position => new LJPoint(Properties["x"].AsNumber(), Properties["y"].AsNumber());
		
		
		//an entity is a list of components and properties.
		//the system loops through all entities, and, for each component, calls the appropriate hooks with the entity properties as part of the stack context.
		//so like, basically 'object oriented' ish?
		public List<Component> Components;
		//true when scene has added entity.
		public bool Loaded { get; private set; }
		private EntityDefinition _definition;
		
		public Entity(EntityDefinition definition, Game game, Scene scene) : base(game, scene)
		{
			_definition = definition;
		}

		public void SetLoaded(bool loaded)
		{
			Loaded = loaded;
		}

		//calls the render function on itself, then on all of it's components in order.
		public override void CallRender()
		{
			base.CallRender();
			foreach (var comp in Components)
			{
				comp.CallRender();
			}
		}
	
		public override bool TryGetProperty(string id, [NotNullWhen(true)] out RuntimeObject expr)
		{
			if (Properties.TryGetValue(id, out expr))
			{
				return true;
			}
			else
			{
				return _game.TryGetProperty(id, out expr);
			}
		}

		
}