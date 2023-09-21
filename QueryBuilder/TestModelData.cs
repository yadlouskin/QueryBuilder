namespace QueryBuilder
{

/*
{
    Name: "Minsk",
    Population: 1995000,
    CityDate: 20230909,
    BasedDate: 10670101,
    Famous: ["National library", "Troitskoe", "Nemiga"],
    Structure:
    {
        Districsts: 9,
        MainStreet: "Independence Avenue",
        Rivers:[
            {
                Name: "Svislach",
                Length: 327,
                Navigable: true
            }, {
                Name: "Nemiga",
                Length: 5,
                Navigable: false
            }
        ]
    }
},{
    Name: "Brest",
    Population: 340000,
    CityDate: 20230728,
    BasedDate: 10170101,
    Famous: ["Brest Fortress"],
    Structure:
    {
        Districsts: 2,
        MainStreet: "Masherov Avenue",
        Rivers:[
            {
                Name: "Bug",
                Length: 774,
                Navigable: true
            }, {
                Name: "Mukhavets",
                Length: 113,
                Navigable: true
            }
        ]
    }
},{
    Name: "Vitebsk",
    Population: 360000,
    CityDate: 20230624,
    BasedDate: 10210101,
    Famous: ["Summer Amphitheatre", "Slavic Bazaar"],
    Structure:
    {
        Districsts: 3,
        MainStreet: "Leningradskaya Street",
        Rivers:[
            {
                Name: "Western Dvina",
                Length: 1020,
                Navigable: true
            }
        ]
    }
}
*/

    public class City
    {
        public string Name { get; set; }
        public int Population { get; set; }
        public int CityDate { get; set; }
        public int BasedDate { get; set; }
        public List<string> Famous { get; set; } = new List<string>();
        public Description Structure { get; set; }        
    }

    public class Description
    {
        public short Districsts { get; set; }
        public string MainStreet { get; set; }
        public List<River> Rivers { get; set; } = new List<River>();
    }

    public class River
    {
        public string Name { get; set;}
        public int Length { get; set;}
        public bool Navigable { get; set; }
    }

}