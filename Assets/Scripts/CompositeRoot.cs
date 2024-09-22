using System;
using System.Collections.Generic;
using System.Linq;
using Bank;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompositeRoot : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CurrencyView _view;
    [SerializeField] private List<Enemy> _enemies;
    private CurrencyPresenter _currencyPresenter;
    private Bank.Bank _bank;

    private List<IDisposable> _disposables = new();
    private LevelEndValidator _validator;

    private void Awake()
    {
        BindCurrency();
        BindLevelEndValidator();
    }

    private void BindCurrency()
    {
        _bank = new Bank.Bank();
        _currencyPresenter = new CurrencyPresenter(_bank, _view);
        _disposables.Add(_currencyPresenter);
        _currencyPresenter.Initialize();
        _player.Initialize(_bank);
    }

    private void BindLevelEndValidator()
    {
        _validator = new LevelEndValidator(_bank, _enemies);
    }

    private void Update()
    {
        _validator.Validate();
    }

    private void OnDestroy() =>
        _disposables.ForEach(x => x.Dispose());
}

public class LevelEndValidator : IDisposable
{
    private Bank.Bank _bank;
    private List<Enemy> _enemies;
    private List<IRule> _rules = new();
    public LevelEndValidator(Bank.Bank bank, List<Enemy> enemies)
    {
        _enemies = enemies;
        _bank = bank;
        _rules.Add(new CurrencyRule(_bank, 5));
        var rule = new EnemiesRule(_enemies, _enemies.Count);
        rule.Initialize();
        _rules.Add(rule);
    }

    public void Validate()
    {
        if (!_rules.All(x => x.IsCompleted))
            return;
        SceneManager.LoadScene(2);
    }

    public void Dispose() =>
        _rules.ForEach(x => x.Dispose());
}

internal class EnemiesRule : IRule, IInitiazable
{
    private List<Enemy> _enemies;
    private int _reference;
    private int _count;
    public bool IsCompleted => _count == _reference;

    public EnemiesRule(List<Enemy> enemies, int reference)
    {
        _reference = reference;
        _enemies = enemies;
    }

    public void Initialize()
    {
        _enemies.ForEach(x =>
        {
            x.Died += OnDied;
        });
    }

    private void OnDied(Enemy enemy)
    {
        _count++;
        enemy.Died -= OnDied;
    }
    public void Dispose()
    { }
}

public class CurrencyRule : IRule
{
    private Bank.Bank _bank;
    private int _reference;

    public bool IsCompleted => _bank.Currency == _reference;
    public CurrencyRule(Bank.Bank bank, int reference)
    {
        _reference = reference;
        _bank = bank;
    }
    public void Dispose()
    {
    }
}

public interface IRule : IDisposable
{
    public bool IsCompleted { get; }
}

public interface IInitiazable
{
    public void Initialize();
}