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
    },
    Field1: 'the same value',
    Field2: 'the same value',
    FieldDec1: NumberDecimal('30'),
    FieldDec2: NumberDecimal('1.5')
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
    },
    Field1: 'the same value',
    Field2: 'the same value2',
    FieldDec1: NumberDecimal('50'),
    FieldDec2: NumberDecimal('2.5')
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
    },
    Field1: 'the same value',
    Field2: 'the same value2',
    FieldDec1: NumberDecimal('40'),
    FieldDec2: NumberDecimal('40')
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
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public decimal FieldDec1 { get; set; }
        public decimal FieldDec2 { get; set; }
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