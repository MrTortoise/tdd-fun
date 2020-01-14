using System;
using System.Collections.Generic;
using System.Linq;
using TestDrivingFun.Console;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class EsTestBase
    {
        protected Message Message = DataGenerator.TestMessage();

        public List<Event> Execute(IEnumerable<Event> given, Command when, List<Event> then,
            Func<IEnumerable<Event>, IAggregate> underTestFactory) 
        {
            var aggregate = underTestFactory(given);
            var result = aggregate.Handle(when).ToList();
            Assert.Equal(then.Count(), result.Count);
            foreach (var e in result)
            {
                Assert.Contains(e.GetType(), then.Select(t=>t.GetType()));
            }

            return result;
        }

        public void Execute(IEnumerable<Event> given, Command when, Type then,
            Func<IEnumerable<Event>, IAggregate> underTestFactory)
        {
            var aggregate = underTestFactory(given);
            Assert.Throws(then, () => aggregate.Handle(when));
        }
    }
}