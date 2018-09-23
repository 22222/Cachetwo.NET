using System;

namespace Cachetwo
{
    public class IdNamePair
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ImmutableIdNamePair
    {
        public ImmutableIdNamePair(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }

    public class NoIdSetterIdNamePair
    {
        public int Id { get; private set; }

        public void SetId(int id) => Id = id;

        public string Name { get; set; }
    }

    public class NoIdGetterIdNamePair
    {
        public int Id { private get; set; }

        public int GetId() => Id;

        public string Name { get; set; }
    }
}
