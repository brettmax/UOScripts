using System;
using Server.Accounting;

namespace Server.Engines.MurderPenalty.Tests;

public class TestAccount : IAccount
{
    private readonly Mobile[] _mobiles;

    public TestAccount(int slots = 7)
    {
        _mobiles = new Mobile[slots];
    }

    public Serial Serial => Serial.Zero;
    public bool Deleted => false;
    public bool ShouldSerialize => false;
    public DateTime Created { get; set; }
    public DateTime LastSerialized { get; set; }

    public string Username => "test";
    public string Email { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public int Length => _mobiles.Length;
    public int Limit => _mobiles.Length;
    public int Count
    {
        get
        {
            var count = 0;
            for (var i = 0; i < _mobiles.Length; i++)
            {
                if (_mobiles[i] != null) count++;
            }
            return count;
        }
    }

    public Mobile this[int index]
    {
        get => _mobiles[index];
        set => _mobiles[index] = value;
    }

    public void Delete() { }
    public bool TrySetUsername(string username) => true;
    public void SetPassword(string password) { }
    public bool CheckPassword(string password) => true;
    public void Deserialize(IGenericReader reader) { }
    public void Serialize(IGenericWriter writer) { }
    public bool SavePosition(out int position) { position = 0; return false; }
    public void MarkDirty() { }
    public byte SerializedThread { get; set; }
    public int SerializedPosition { get; set; }
    public int SerializedLength { get; set; }

    public int TotalGold => 0;
    public int TotalPlat => 0;
    public bool DepositGold(int amount) => true;
    public bool DepositPlat(int amount) => true;
    public bool WithdrawGold(int amount) => true;
    public bool WithdrawPlat(int amount) => true;
    public long GetTotalGold() => 0;

    public int CompareTo(IAccount other) => 0;
}
