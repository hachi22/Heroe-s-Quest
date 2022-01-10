using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] Fighter _fighter;

    private void Start()
    {     
            _fighter.Init();      
    }

    public Fighter GetFighter()
    {
        return _fighter;
    }

    private void Update()
    {
        _fighter.Moves = new List<Move>();
        foreach (var move in _fighter.Base.LearnableMoves)
        {
            if (move.Level <= _fighter.Level)
                _fighter.Moves.Add(new Move(move.Base));

            if (_fighter.Moves.Count >= 4)
                break;
        }
    }


}
