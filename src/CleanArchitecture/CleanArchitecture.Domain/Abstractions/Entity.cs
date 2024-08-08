namespace CleanArchitecture.Domain.Abstractions
{
    public abstract class Entity<TEntityId> : IEntity
    {
        protected Entity(){}
        private readonly List<IDomainEvent> _domainEvents = new();
        public TEntityId? Id { get; init; }

        protected Entity(TEntityId id)
        {
            Id = id;
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.ToList();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent _domainEvents){
            this._domainEvents.Add(_domainEvents);
        }

    }
}