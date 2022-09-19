public class Bag
{
    private readonly List<BagCell> _cells;
    private readonly int _capacity;

    public Bag(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _cells = new List<BagCell>();

        _capacity = capacity;
    }

    private int CurrentWeight => _cells.Sum(cell => cell.Weight);

    public IReadOnlyList<IReadOnlyBagCell> Cells => _cells;

    public IEnumerable<Item> GetItems() => _cells.Select(cell => cell.Item).ToList();

    public bool Add(Item item, int count)
    {
        var newCell = new BagCell(item, count);

        if (IsThereEnoughCapacityFor(newCell))
        {
            if (IsItemAlreadyExists(item, out int cellIndex))
            {
                _cells[cellIndex].Merge(newCell);
            }
            else
            {
                _cells.Add(newCell);
            }

            return true;
        }

        return false;
    }

    private bool IsThereEnoughCapacityFor(BagCell newCell)
    {
        return _capacity >= CurrentWeight + newCell.Weight;
    }

    private bool IsItemAlreadyExists(Item item, out int cellIndex)
    {
        cellIndex = _cells.FindIndex(cell => cell.Item == item);

        return cellIndex >= 0;
    }
}

public class BagCell : IReadOnlyBagCell
{
    private readonly Item _item;
    private int _count;

    public BagCell(Item item, int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        _item = item;
        _count = count;
    }

    public int Weight => _item.Weight * _count;
    public Item Item => _item;
    public int Count => _count;

    public void Merge(BagCell newCell)
    {
        if (newCell.Item != _item)
        {
            throw new InvalidOperationException();
        }

        _count += newCell._count;
    }
}

public interface IReadOnlyBagCell
{
    public Item Item { get; }
    public int Count { get; }
}

public class Item
{
    private readonly string _name;
    private readonly int _weight;

    public Item(string name, int weight = 0)
    {
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight));
        }

        _name = name;
        _weight = weight;
    }

    public string Name => _name;
    public int Weight => _weight;
}