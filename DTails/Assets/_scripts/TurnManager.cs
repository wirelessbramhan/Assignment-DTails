using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private List<UnitController> _playerTeam, _enemyTeam;
    [SerializeField] private UnitController _currentUnit;
    [SerializeField] private bool _isPlayerTurn;
    [SerializeField] private int _playerTeamCount, _enemyTeamCount;
    [SerializeField] private UnitController _playerUnitPrefab, _enemyUnitPrefab;
    [SerializeField] private CinemachineTargetGroup _targetGroup;

    IEnumerator Start()
    {
        _playerTeam = new List<UnitController>();
        _enemyTeam = new List<UnitController>();


        for (int i = 0; i < _playerTeamCount; i++)
        {
            var playerUnit = Instantiate(_playerUnitPrefab, new Vector3(i * 2, 0, 0), Quaternion.identity);

            _targetGroup.AddMember(playerUnit.transform, 1, 2);
            playerUnit.SetTeam(UnitTeam.Red);
            playerUnit.SetSpeed(_targetGroup.Targets.Count + 1);

            _playerTeam.Add(playerUnit);
        }

        for (int i = 0; i < _enemyTeamCount; i++)
        {
            var enemyUnit = Instantiate(_enemyUnitPrefab, new Vector3(i * 2, 0, -5), Quaternion.Euler(0, 180, 0));
            enemyUnit.SetTeam(UnitTeam.Blue);
            _targetGroup.AddMember(enemyUnit.transform, 1, 2);
            enemyUnit.SetSpeed(_targetGroup.Targets.Count + 1);

            _enemyTeam.Add(enemyUnit);
        }

        yield return new WaitForSeconds(0.5f);
        GetAttckUnit(_playerTeam);
        _currentUnit.Attack();
        GetAttckUnit(_enemyTeam);
        _currentUnit.Attack();
        yield return null;
    }

    private void GetAttckUnit(List<UnitController> team)
    {
        if (team.Count > 0)
        {
            float highestSpeed = 0;
            //check max speed
            for (int i = 0; i < team.Count; i++)
            {
                if (team[i].GetSpeed() > highestSpeed)
                {
                    _currentUnit = team[i];
                    highestSpeed = team[i].GetSpeed();
                }
            }
        }
    }
}
