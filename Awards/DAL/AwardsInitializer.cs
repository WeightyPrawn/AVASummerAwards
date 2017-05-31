using Awards.DAL;
using Awards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.DAL
{
    public class AwardsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AwardsContext>
    {
        protected override void Seed(AwardsContext context)
        {
            var categories = new List<Category>
            {
                new Category {Name="Category1", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean dapibus augue lectus, sit amet eleifend augue elementum eget. Nullam sed odio eleifend, dictum nisl a, sodales neque. Nulla ac eros sed felis pulvinar ullamcorper sed a erat. Nam auctor, enim sed viverra viverra, orci augue gravida lectus, id vestibulum nisl lectus in lorem. Integer tortor enim, luctus in mi sit amet, luctus elementum turpis. Mauris commodo sed ipsum eu aliquam. Nullam viverra nulla velit, et placerat purus mollis rhoncus. Mauris et consectetur elit, non malesuada quam."},
                new Category {Name="Category2", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean dapibus augue lectus, sit amet eleifend augue elementum eget. Nullam sed odio eleifend, dictum nisl a, sodales neque. Nulla ac eros sed felis pulvinar ullamcorper sed a erat. Nam auctor, enim sed viverra viverra, orci augue gravida lectus, id vestibulum nisl lectus in lorem. Integer tortor enim, luctus in mi sit amet, luctus elementum turpis. Mauris commodo sed ipsum eu aliquam. Nullam viverra nulla velit, et placerat purus mollis rhoncus. Mauris et consectetur elit, non malesuada quam."},
                new Category {Name="Category3", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean dapibus augue lectus, sit amet eleifend augue elementum eget. Nullam sed odio eleifend, dictum nisl a, sodales neque. Nulla ac eros sed felis pulvinar ullamcorper sed a erat. Nam auctor, enim sed viverra viverra, orci augue gravida lectus, id vestibulum nisl lectus in lorem. Integer tortor enim, luctus in mi sit amet, luctus elementum turpis. Mauris commodo sed ipsum eu aliquam. Nullam viverra nulla velit, et placerat purus mollis rhoncus. Mauris et consectetur elit, non malesuada quam."},
                new Category {Name="Category4", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean dapibus augue lectus, sit amet eleifend augue elementum eget. Nullam sed odio eleifend, dictum nisl a, sodales neque. Nulla ac eros sed felis pulvinar ullamcorper sed a erat. Nam auctor, enim sed viverra viverra, orci augue gravida lectus, id vestibulum nisl lectus in lorem. Integer tortor enim, luctus in mi sit amet, luctus elementum turpis. Mauris commodo sed ipsum eu aliquam. Nullam viverra nulla velit, et placerat purus mollis rhoncus. Mauris et consectetur elit, non malesuada quam."}
            };

            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var nominees = new List<Nominee>
            {
                new Nominee { CategoryID=1, Name="Nominee1", Email="nominee1@avanade.com" },
                new Nominee { CategoryID=1, Name="Nominee2", Email="nominee2@avanade.com" },
                new Nominee { CategoryID=2, Name="Nominee1", Email="nominee1@avanade.com" },
                new Nominee { CategoryID=3, Name="Nominee2", Email="nominee2@avanade.com" }
            };
            nominees.ForEach(s => context.Nominees.Add(s));
            context.SaveChanges();

            var nominations = new List<Nomination>
            {
                new Nomination {NomineeID=1, Nominator="nominator1@avanade.com", Reason="Reason1", Anonymous=false },
                new Nomination {NomineeID=1, Nominator="nominator2@avanade.com", Reason="Reason2", Anonymous=false },
                new Nomination {NomineeID=2, Nominator="nominator3@avanade.com", Reason="Reason1", Anonymous=false },
                new Nomination {NomineeID=2, Nominator="nominator1@avanade.com", Reason="Reason2", Anonymous=false },
                new Nomination {NomineeID=3, Nominator="nominator2@avanade.com", Reason="Reason1", Anonymous=true },
                new Nomination {NomineeID=3, Nominator="nominator4@avanade.com", Reason="Reason2", Anonymous=false },
                new Nomination {NomineeID=3, Nominator="nominator3@avanade.com", Reason="Reason3", Anonymous=false },
                new Nomination {NomineeID=4, Nominator="nominator5@avanade.com", Reason="Reason1", Anonymous=true }
            };
            nominations.ForEach(s => context.Nominations.Add(s));
            context.SaveChanges();

            var votes = new List<Vote>
            {
                new Vote { NomineeID=1, Voter="voter1@avanade.com" },
                new Vote { NomineeID=2, Voter="voter2@avanade.com" },
                new Vote { NomineeID=2, Voter="voter3@avanade.com" },
                new Vote { NomineeID=4, Voter="voter4@avanade.com" },
            };
            votes.ForEach(s => context.Votes.Add(s));
            context.SaveChanges();
        }
    }
}