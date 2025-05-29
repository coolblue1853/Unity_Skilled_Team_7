using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    Player player;
    PlayerStatus playerStatus;
    PlayerAnimationHandler playerAnim;

    [SerializeField] WeaponController[] weapons; // 나중에 Resources 에서 가져오는 걸로
    [SerializeField] List<WeaponController> unlockWeapons = new List<WeaponController>();
    [SerializeField] WeaponController curWeapon;
    [SerializeField] Transform throwPoint;

    private bool isAiming = false;
    private TrajectoryController trajectoryController;

    public void Init(Player player, PlayerStatus playerStatus, PlayerAnimationHandler playerAnim)
    {
        this.player = player;
        this.playerStatus = playerStatus;
        this.playerAnim = playerAnim;

        UnlockWeapon(0);
        UnlockWeapon(1);

        trajectoryController = transform.parent.GetComponentInChildren<TrajectoryController>();

        if (trajectoryController)
            trajectoryController.Init(throwPoint);
    }

    public void UnlockWeapon(int idx)
    {
        GameObject obj = Instantiate(weapons[idx].gameObject, throwPoint);

        WeaponController weapon = obj.GetComponent<WeaponController>();
        weapon.ShowModel(false);
        weapon.Init(throwPoint);

        unlockWeapons.Add(weapon);
    }

    public void ChangeWeapon(int idx)
    {
        if (curWeapon != null)
        {
            curWeapon.ShowModel(false);
        }

        if (idx > -1 && idx < unlockWeapons.Count)
        {
            curWeapon = unlockWeapons[idx];
            curWeapon.ShowModel(true);
        }
    }

    public void StartAiming()
    {
        if (curWeapon == null || curWeapon.IsCoolDown) return;

        isAiming = true;
        trajectoryController.Show();
    }

    public void StopAiming()
    {
        isAiming = false;
        trajectoryController.Hide();
    }

    public void CheckThrow()
    {
        if (!isAiming || !playerStatus.UseStamina(curWeapon.WeaponDataSO.useStamina)) return;

        if (player.CurState != PlayerState.Vehicle)
        {
            player.ChangeState(PlayerState.Throw);
            playerAnim.PlayThrow();
        }
        else
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        isAiming = false;
        trajectoryController.Hide();

        Vector3 direction = trajectoryController.GetAimDirectionForce(out float force);
        curWeapon.ThrowWeapon(direction, force);

        if(player.CurState != PlayerState.Vehicle)
            player.ChangeState(PlayerState.Idle);
    }
}