# QueryBuilder

<br />

# Тех задание

На основе Jquery Builder или чего либо другого разработать страницу с возможностью строить запросы Mongodb через визуальную часть.

<br />

### Случай 1:
Свойство из класса операция сравнения(=,<,>,<>......) со значением. (BusinessDate = 1 or StoreId > 44)

### Случай 2:
Свойство из коллекции из класса операция сравнения со значением. (Items.LineNum = 1 or Items.ItemFlags.IsQtyPos = false)

### Случай 3:
Свойство Бизнес даты(BusinessDate) операция сравнения с динамическим значением (today, yesterday, this week)\
(BusinessDate = 'today' но это будет относительно того дня когда оно будет запущено. Если сегодня 05.09.2023 то today должно быть преобразовано в 20230905)

### Случай 4:
Свойство из класса операция сравнения Свойство из класса ("BusinessDate" (equal) "SystemDate")

### Случай 5:
Выражение на основе свойств из класса операция сравнения со значением "Items.RetailPrice" (divided by) “Items.SellingPrice” (greater) “.25”

<br />

## Пример данных
```c#
public class Sale
{
    public int BusinessDate { get; set; }
    public int StoreId { get; set; }
    public string TerminalId { get; set; }
    public short TransactionId { get; set; }
    public bool IsPostVoided { get; set; }
    public bool IsUnbalanced { get; set; }
    public TransactionType TranType { get; set; }
    public List<ItemTLog> Items { get; set; } = new List<ItemTLog>();
}

public class TransactionType
{
    [Required]
    public bool IsSale { get; set; }
    [Required]
    public bool IsReturn { get; set; }
    public bool IsExchange { get; set; }
    public bool IsLayaway { get; set; }
    public bool IsSpecOrder { get; set; }
    public bool IsEmployee { get; set; }
    public bool IsFunction { get; set; }
    public bool IsVoidDur { get; set; }
    public bool IsSuspended { get; set; }
}

public class ItemTLog
{
    [Required]
    public int LineNum { get; set; }
    [Required]
    public ItemFlags ItemFlags { get; set; }
    [Required]
    public string ItemId { get; set; }
    [Required]
    public string Description { get; set; }
    public string DepartmentId { get; set; }
    public string ClassId { get; set; }
    public string SubClassId { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal SellingPrice { get; set; }
}

public class ItemFlags
{
    public bool IsQtyPos { get; set; }
    public bool IsQtyNeg { get; set; }
    public bool IsPricePos { get; set; }
    public bool IsPriceNeg { get; set; }
    public bool IsItemOnSale { get; set; }
    public bool IsUserPrice { get; set; }
    public bool IsPriceOverRide { get; set; }
    [Required]
    public bool IsTaxable { get; set; }
    public bool IsItemCorrect { get; set; }
}
```