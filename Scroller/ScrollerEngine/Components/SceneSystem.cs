using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel
    /// Provides a system that's contained within a Scene.
    /// A system applies operations to Entities within the Scene, such as providing physics logic.
    /// While a Component can apply updates to itself, or render itself, it's recommended to use a System when dealing with multiple Entities.
    /// </summary>
    public abstract class SceneSystem : SceneObject
    {
        private List<ComponentFilter> _Filters = new List<ComponentFilter>();
        private HashSet<Entity> _EntitiesTracked = new HashSet<Entity>();

        protected IEnumerable<T> GetFilteredComponents<T>() where T : Component
        {
            ComponentFilter filter = _Filters.FirstOrDefault(c => c.ComponentType == typeof(T));
            if (filter.ComponentType == null)
            {
                // Filter not found, default(ComponentFilter).
                // We're not tracking this type, so find every Component and return it.
                // If we find a Component, make sure that Entity is being tracked.
                var components = new List<Component>();
                foreach (var entity in Scene.Entities)
                {
                    bool any = false;
                    foreach (var component in entity.Components)
                    {
                        var componentType = component.GetType();
                        if (componentType == typeof(T) || componentType.IsSubclassOf(typeof(T)))
                        {
                            components.Add(component);
                            any = true;
                        }
                    }
                    if (any)
                        RegisterEntity(entity);
                }
                filter = new ComponentFilter(typeof(T), components);
                this._Filters.Add(filter);
            }
            return filter.Components.Select(c => (T)c);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Scene.ObjectAdded += Scene_ObjectAdded;
            Scene.ObjectRemoved += Scene_ObjectRemoved;
        }

        protected override void OnDispose()
        {
            Scene.ObjectAdded -= Scene_ObjectAdded;
            Scene.ObjectRemoved -= Scene_ObjectRemoved;
            base.OnDispose();
        }

        void Scene_ObjectAdded(Entity obj)
        {
            RegisterEntity(obj);
        }

        void Scene_ObjectRemoved(Entity obj)
        {
            UnregisterEntity(obj);
        }

        private IEnumerable<ComponentFilter> GetFiltersForType(Type componentType)
        {
            // This can be multiple filters because of filter types deriving from Type.
            foreach (var filter in this._Filters)
            {
                var type = filter.ComponentType;
                if (type == componentType || componentType.IsSubclassOf(type))
                    yield return filter;
            }
        }

        private void RegisterEntity(Entity entity)
        {
            if (!_EntitiesTracked.Add(entity))
                return;
            foreach (var component in entity.Components)
            {
                var componentType = component.GetType();
                foreach (var filter in GetFiltersForType(componentType))
                    filter.Components.Add(component);
            }
            entity.Components.ComponentAdded += Components_ComponentAdded;
            entity.Components.ComponentRemoved += Components_ComponentRemoved;
        }

        private void UnregisterEntity(Entity entity)
        {
            if (!_EntitiesTracked.Contains(entity))
                return;
            foreach (var component in entity.Components)
            {
                var componentType = component.GetType();
                foreach (var filter in GetFiltersForType(componentType))
                {
                    bool result = filter.Components.Remove(component);
                    if (!result)
                        throw new KeyNotFoundException();
                }
            }
            _EntitiesTracked.Remove(entity);
            entity.Components.ComponentAdded -= Components_ComponentAdded;
            entity.Components.ComponentRemoved -= Components_ComponentRemoved;
        }

        void Components_ComponentRemoved(Component obj)
        {
            foreach (var filter in GetFiltersForType(obj.GetType()))
            {
                bool result = filter.Components.Remove(obj);
                if (!result)
                    throw new KeyNotFoundException();
            }
        }

        void Components_ComponentAdded(Component obj)
        {
            foreach (var filter in GetFiltersForType(obj.GetType()))
                filter.Components.Add(obj);
        }

        private struct ComponentFilter
        {
            public Type ComponentType;
            public List<Component> Components;

            public ComponentFilter(Type type, List<Component> components)
            {
                this.ComponentType = type;
                this.Components = components;
            }
        }
    }
}
