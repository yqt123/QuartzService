namespace QuartzNancy.Model
{
    public class TestClass
    {
        public TestClass()
        { }
        public TestClass(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public string ID { get; set; }
        public string Name { get; set; }
    }
}
