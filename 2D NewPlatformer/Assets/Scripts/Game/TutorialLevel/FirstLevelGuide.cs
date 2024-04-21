using System;
using System.Collections;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo

public class FirstLevelGuide : MonoBehaviour
{
    [SerializeField] private PlayerController currentPlayer;

    [SerializeField] private ApproximationTrigger collectableItemApproximationTrigger;
    [SerializeField] private Transform collectableItemGuideTransform;
    [SerializeField] private FirstCollectableObject firstCollectableObject;
    [SerializeField] private SpikesApproximationTrigger spikesApproximationTrigger;
    [SerializeField] private GuidesSO sipkesDamageGuideSO;
    [SerializeField] private OnSpikesDamage spikesDamageGuideInterafceButton;
    [SerializeField] private ApproximationTrigger movingPlatformApproximationTrigeer;
    [SerializeField] private GuideAddInteractButton movingPlatform1AddInteractButton;
    [SerializeField] private ApproximationTrigger middleMovingPlatformApproximationTrigger;
    private bool isFirstConditionForMovingPlatform1Complete;
    [SerializeField] private GuidesSO movingPlatformInteractGuideSO;
    [SerializeField] private GuidesSO movingPlatformDestinationsGuideSO;
    [SerializeField] private ApproximationTrigger movingPlatformUpperPositionApproximationTrigger;
    [SerializeField] private MovingPlatform firstMovingPlatform;
    [SerializeField] private GuidesSO doubleJumpStartGuideSO;
    private bool isWalkingLeft;
    [SerializeField] private ApproximationTrigger doubleJumpPointApproximationTrigger;
    [SerializeField] private GuidesSO doubleJumpJumpGuideSO;
    [SerializeField] private ApproximationTrigger doubleJumpStopWalkingApproximationTrigger;
    [SerializeField] private GuideAddInteractButton callbackLeverAddInteractButton;
    [SerializeField] private GuidesSO leverGuideSO;
    [SerializeField] private ApproximationTrigger tramplineApproximationTrigger;
    [SerializeField] private SandMud sandMud;
    [SerializeField] private GuidesSO sandMudGuide;
    [SerializeField] private ApproximationTrigger movableHeadApproximationTrigger;
    [SerializeField] private GuidesSO movableHeadGuide;
    [SerializeField] private MovableHead movableHead;
    [SerializeField] private PressurePlate pressurePlate;
    [SerializeField] private ApproximationTrigger fallingPlatformApproximationTrigger;
    [SerializeField] private ApproximationTrigger middleFallingPlatformApproximationTrigger;
    [SerializeField] private GuidesSO fallingPlatformGuideSO;

    [SerializeField] private ApproximationTrigger groundUnderFallingPlatfromApproximationTrigger;

    //Отслеживать получаемый урон от пил и зажигалки
    [SerializeField] private ApproximationTrigger breakableBlockApproximationTrigger;
    [SerializeField] private BreakableBlock breakableBlock;

    [SerializeField]
    private MovingPlatform
        movingPlatformToStoreBlock; //Отключить взаимодействие до того, пока нет внутри BreakableBlock

    [SerializeField] private GuidesSO breakableBlockMoveGuideSO; //Заставляем сдвинуть на платформу

    [SerializeField]
    private GuideAddInteractButton breakableBlockAddInteractButton; //Ждем пока можно будет поместить внутрь

    [SerializeField] private GuidesSO breakableBlockStoreGuideSO; //Заставляем поместить внутрь

    [SerializeField]
    private ApproximationTrigger middleMovingPlatform2ApproximationTrigger; //Ждем пока дойдет до центра платформы

    //+ включаем взаиомодействие
    [SerializeField] private GuideAddInteractButton movingPlatform2AddInteractButton; //Ждем начала движения
    [SerializeField] private GuidesSO breakableBlockMovingOnPlatformGuideSO;
    [SerializeField] private GuidesSO breakableBlockBreakGuideSO; //Ждем прибытия + заставляем достать блок


    private void Start()
    {
        PlayerChangeController.Instance.OnPlayerChange += PLayerChangeController_OnPlayerChange;

        //Приближение к первому фрукту
        collectableItemApproximationTrigger.OnApproximationTrigger += CollectableItemTrigger_OnApproximationTrigger;
    }

    private void PLayerChangeController_OnPlayerChange(object sender, EventArgs e)
    {
        currentPlayer = PlayerChangeController.Instance.GetCurrentPlayerController();
    }

    //Приближение к первому фрукту
    private void CollectableItemTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.LockAllBindings();
        collectableItemGuideTransform.gameObject.SetActive(true);
        firstCollectableObject.OnCollect += FirstCollectableObject_OnCollect;
        StartCoroutine(WaitForUnlockMovementAfterCollectableItemTrigger());
        collectableItemApproximationTrigger.OnApproximationTrigger -= CollectableItemTrigger_OnApproximationTrigger;
    }

    private IEnumerator WaitForUnlockMovementAfterCollectableItemTrigger()
    {
        yield return new WaitForSeconds(1f);
        currentPlayer.UnlockBinding(Input.Binding.MoveRight);
    }

    //Сбор первого фрукта
    private void FirstCollectableObject_OnCollect(object sender, EventArgs e)
    {
        currentPlayer.UnlockAllBindings();
        collectableItemGuideTransform.gameObject.SetActive(false);

        spikesApproximationTrigger.OnApproximationTrigger += SpikesTrigger_OnApproximationTrigger;
        firstCollectableObject.OnCollect -= FirstCollectableObject_OnCollect;
    }

    //Приближение к шипам
    private void SpikesTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.LockAllBindings();
        currentPlayer.UnlockBinding(Input.Binding.MoveRight);
        PlayerController.OnPlayerHealthChange += PlayerController_OnPlayerHealthChange;
        spikesApproximationTrigger.OnApproximationTrigger -= SpikesTrigger_OnApproximationTrigger;
    }

    //Получение урона от шипов
    private void PlayerController_OnPlayerHealthChange(object sender, PlayerController.OnPlayerHealthChangeEventArgs e)
    {
        Time.timeScale = 0f;
        GuideInterface.Instance.ShowGuide(sipkesDamageGuideSO);
        spikesDamageGuideInterafceButton.OnButtonPressed += OnSpikesDamageButton_OnButtonPressed;
    }

    //Нажатие на кнопку в гайде после получения урона
    private void OnSpikesDamageButton_OnButtonPressed(object sender, EventArgs e)
    {
        movingPlatformApproximationTrigeer.OnApproximationTrigger +=
            MovingPlatformApproximationTrigeer_OnApproximationTrigger;
    }

    //Приближение к двигающейся платформе
    private void MovingPlatformApproximationTrigeer_OnApproximationTrigger(object sender, EventArgs e)
    {
        PlayerController.OnPlayerHealthChange -= PlayerController_OnPlayerHealthChange;
        spikesDamageGuideInterafceButton.OnButtonPressed -= OnSpikesDamageButton_OnButtonPressed;

        firstMovingPlatform.ChangeInteractionLockState(true);

        currentPlayer.LockAllBindings();
        currentPlayer.UnlockBinding(Input.Binding.MoveRight);
        movingPlatform1AddInteractButton.OnInteractButtonAdded += GuideAddInteractButton_OnInteractButtonAdded;
        middleMovingPlatformApproximationTrigger.OnApproximationTrigger +=
            MiddleMovingPlatformApproximationTrigger_OnApproximationTrigger;

        movingPlatformApproximationTrigeer.OnApproximationTrigger -=
            MovingPlatformApproximationTrigeer_OnApproximationTrigger;
    }

    //Приближение к центру двигающейся платформы
    private void MiddleMovingPlatformApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        if (isFirstConditionForMovingPlatform1Complete)
        {
            currentPlayer.LockAllBindings();
            GuideInterface.Instance.ShowGuide(movingPlatformInteractGuideSO);
            movingPlatform1AddInteractButton.OnInteractButtonRemoved += GuideAddInteractButton_OnInteractButtonRemoved;
            firstMovingPlatform.ChangeInteractionLockState(false);
        }
        else
        {
            isFirstConditionForMovingPlatform1Complete = true;
        }

        middleMovingPlatformApproximationTrigger.OnApproximationTrigger -=
            MiddleMovingPlatformApproximationTrigger_OnApproximationTrigger;
    }

    //Появление кнопки движения у двигающейся платформы
    private void GuideAddInteractButton_OnInteractButtonAdded(object sender, EventArgs e)
    {
        isFirstConditionForMovingPlatform1Complete = true;

        movingPlatform1AddInteractButton.OnInteractButtonAdded -= GuideAddInteractButton_OnInteractButtonAdded;
    }

    //Начало движения
    private void GuideAddInteractButton_OnInteractButtonRemoved(object sender, EventArgs e)
    {
        movingPlatform1AddInteractButton.OnInteractButtonAdded += GuideAddInteractButton_OnInteractButtonAdded2;
        GuideInterface.Instance.Hide();

        movingPlatform1AddInteractButton.OnInteractButtonRemoved -= GuideAddInteractButton_OnInteractButtonRemoved;
    }

    //Прибытие к точке назначения
    private void GuideAddInteractButton_OnInteractButtonAdded2(object sender, EventArgs e)
    {
        GuideInterface.Instance.ShowGuide(movingPlatformDestinationsGuideSO);
        movingPlatform1AddInteractButton.OnInteractButtonRemoved += GuideAddInteractButton_OnInteractButtonRemoved2;
        movingPlatform1AddInteractButton.OnInteractButtonAdded -= GuideAddInteractButton_OnInteractButtonAdded2;
    }

    //Начало движения к верхней точке
    private void GuideAddInteractButton_OnInteractButtonRemoved2(object sender, EventArgs e)
    {
        GuideInterface.Instance.Hide();
        movingPlatformUpperPositionApproximationTrigger.OnApproximationTrigger +=
            MovingPlatformUpperPositionTrigger_OnApproximationTrigger;

        movingPlatform1AddInteractButton.OnInteractButtonRemoved -= GuideAddInteractButton_OnInteractButtonRemoved2;
    }

    //Приближение к верхней точке движения
    private void MovingPlatformUpperPositionTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        GuideInterface.Instance.ShowGuide(doubleJumpStartGuideSO);
        GuideInterface.Instance.OnGuideClose += GuideInterface_OnGuideClose;
        firstMovingPlatform.ChangeInteractionLockState(true);

        movingPlatformUpperPositionApproximationTrigger.OnApproximationTrigger -=
            MovingPlatformUpperPositionTrigger_OnApproximationTrigger;
    }

    //Нажатие кнопок для двойного прыжка
    private void GuideInterface_OnGuideClose(object sender, EventArgs e)
    {
        doubleJumpPointApproximationTrigger.OnApproximationTrigger +=
            DoubleJumpPointApproximationTrigger_OnApproximationTrigger;
        isWalkingLeft = true;
        currentPlayer.ForcedJump();

        GuideInterface.Instance.OnGuideClose -= GuideInterface_OnGuideClose;
    }

    private void Update()
    {
        if (isWalkingLeft)
            currentPlayer.ForcedMoveLeft();
    }

    //Приближение к точке второго прыжка
    private void DoubleJumpPointApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        GuideInterface.Instance.ShowGuide(doubleJumpJumpGuideSO);

        GuideInterface.Instance.OnGuideClose += GuideInterafce_OnGuideClose;

        doubleJumpPointApproximationTrigger.OnApproximationTrigger -=
            DoubleJumpPointApproximationTrigger_OnApproximationTrigger;
    }

    //Нажатие прыжка в воздухе
    private void GuideInterafce_OnGuideClose(object sender, EventArgs e)
    {
        currentPlayer.ForcedJump();
        doubleJumpStopWalkingApproximationTrigger.OnApproximationTrigger +=
            StopWalkingApproximationPoing_OnApproximationTrigger;

        GuideInterface.Instance.OnGuideClose -= GuideInterafce_OnGuideClose;
    }

    //Точка конца движения в воздухе
    private void StopWalkingApproximationPoing_OnApproximationTrigger(object sender, EventArgs e)
    {
        isWalkingLeft = false;

        firstMovingPlatform.ChangeInteractionLockState(false);

        callbackLeverAddInteractButton.OnInteractButtonAdded += CallbackLeverAddInteractButton_OnInteractButtonAdded;

        doubleJumpStopWalkingApproximationTrigger.OnApproximationTrigger -=
            StopWalkingApproximationPoing_OnApproximationTrigger;
    }

    //Появление кнопки вызова платформы назад
    private void CallbackLeverAddInteractButton_OnInteractButtonAdded(object sender, EventArgs e)
    {
        GuideInterface.Instance.ShowGuide(leverGuideSO);
        callbackLeverAddInteractButton.OnInteractButtonRemoved +=
            CallbackLeverAddInteractButton_OnInteractButtonRemoved;

        callbackLeverAddInteractButton.OnInteractButtonAdded -= CallbackLeverAddInteractButton_OnInteractButtonAdded;
    }

    //Успешный вызов платформы назад
    private void CallbackLeverAddInteractButton_OnInteractButtonRemoved(object sender, EventArgs e)
    {
        GuideInterface.Instance.Hide();
        currentPlayer.UnlockAllBindings();
        tramplineApproximationTrigger.OnApproximationTrigger += TramplineApproximationTrigger_OnApproximationTrigger;

        callbackLeverAddInteractButton.OnInteractButtonRemoved -=
            CallbackLeverAddInteractButton_OnInteractButtonRemoved;
    }

    //Приближение к трамплину
    private void TramplineApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        sandMud.OnPlayerSlowDown += SandMud_OnPlayerSlowDown;
        movableHeadApproximationTrigger.OnApproximationTrigger +=
            MovableHeadApproximationTrigger_OnApproximationTrigger;

        tramplineApproximationTrigger.OnApproximationTrigger -= TramplineApproximationTrigger_OnApproximationTrigger;
    }

    //Падение вниз с паркура
    private void SandMud_OnPlayerSlowDown(object sender, EventArgs e)
    {
        GuideInterface.Instance.ShowGuide(sandMudGuide);
    }

    //Приближение к двигающейся голове
    private void MovableHeadApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.LockAllBindings();
        currentPlayer.UnlockBinding(Input.Binding.MoveRight);

        GuideInterface.Instance.ShowGuide(movableHeadGuide);

        pressurePlate.OnPressurePlateActivated += PressurePlate_OnPressurePlateActivated;

        sandMud.OnPlayerSlowDown -= SandMud_OnPlayerSlowDown;
        movableHeadApproximationTrigger.OnApproximationTrigger -=
            MovableHeadApproximationTrigger_OnApproximationTrigger;
    }

    //Голова на плите
    private void PressurePlate_OnPressurePlateActivated(object sender, EventArgs e)
    {
        movableHead.ChangeIsCanBeMovedState(false);

        GuideInterface.Instance.Hide();

        currentPlayer.UnlockAllBindings();

        fallingPlatformApproximationTrigger.OnApproximationTrigger +=
            FallingPlatformApproximationTrigger_OnApproximationTrigger;

        pressurePlate.OnPressurePlateActivated -= PressurePlate_OnPressurePlateActivated;
    }

    //Приближение к падающей платформе
    private void FallingPlatformApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.LockAllBindings();
        currentPlayer.UnlockBinding(Input.Binding.MoveRight);

        middleFallingPlatformApproximationTrigger.OnApproximationTrigger +=
            MiddleFallingPlatformApproximationTrigger_OnApproximationTrigger;

        GuideInterface.Instance.ShowGuide(fallingPlatformGuideSO);

        fallingPlatformApproximationTrigger.OnApproximationTrigger -=
            FallingPlatformApproximationTrigger_OnApproximationTrigger;
    }

    //В центре падающей платформы
    private void MiddleFallingPlatformApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.LockAllBindings();

        groundUnderFallingPlatfromApproximationTrigger.OnApproximationTrigger +=
            GroundUnderFallingPlatfromApproximationTrigger_OnApproximationTrigger;

        middleFallingPlatformApproximationTrigger.OnApproximationTrigger -=
            MiddleFallingPlatformApproximationTrigger_OnApproximationTrigger;
    }

    //Падание вниз с падающей платформы
    private void GroundUnderFallingPlatfromApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.UnlockAllBindings();

        GuideInterface.Instance.Hide();

        breakableBlockApproximationTrigger.OnApproximationTrigger +=
            BreakableBlockApproximationTrigger_OnApproximationTrigger;

        groundUnderFallingPlatfromApproximationTrigger.OnApproximationTrigger -=
            GroundUnderFallingPlatfromApproximationTrigger_OnApproximationTrigger;
    }

    //Приближение к ломаемому блоку
    private void BreakableBlockApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        GuideInterface.Instance.ShowGuide(breakableBlockMoveGuideSO);

        currentPlayer.LockAllBindings();
        currentPlayer.UnlockBinding(Input.Binding.MoveRight);
        movingPlatformToStoreBlock.ChangeInteractionLockState(true);

        breakableBlockAddInteractButton.OnInteractButtonAdded += BreakableBlockAddInteractButton_OnInteractButtonAdded;

        breakableBlockApproximationTrigger.OnApproximationTrigger -=
            BreakableBlockApproximationTrigger_OnApproximationTrigger;
    }

    //Ломаемый блок на двигающейся платформе
    private void BreakableBlockAddInteractButton_OnInteractButtonAdded(object sender, EventArgs e)
    {
        GuideInterface.Instance.Hide();
        GuideInterface.Instance.ShowGuide(breakableBlockStoreGuideSO);

        breakableBlockAddInteractButton.OnInteractButtonRemoved +=
            BreakableBlockAddInteractButton_OnInteractButtonRemoved;

        breakableBlock.ChangeIsCanBeMovedState(false);

        breakableBlockAddInteractButton.OnInteractButtonAdded -= BreakableBlockAddInteractButton_OnInteractButtonAdded;
    }

    //Ломаемый блок внутри двигающейся платформы
    private void BreakableBlockAddInteractButton_OnInteractButtonRemoved(object sender, EventArgs e)
    {
        GuideInterface.Instance.Hide();
        GuideInterface.Instance.ShowGuide(breakableBlockMovingOnPlatformGuideSO);

        StartCoroutine(ChangeCantStoreBlock());

        middleMovingPlatform2ApproximationTrigger.OnApproximationTrigger +=
            MiddleMovingPlatform2ApproximationTrigger_OnApproximationTrigger;

        breakableBlockAddInteractButton.OnInteractButtonRemoved -=
            BreakableBlockAddInteractButton_OnInteractButtonRemoved;
    }

    private IEnumerator ChangeCantStoreBlock()
    {
        yield return new WaitForSeconds(0.1f);
        breakableBlock.ChangeIsCanBeStoredState(false);
    }

    //В центре двигающейся плафтормы
    private void MiddleMovingPlatform2ApproximationTrigger_OnApproximationTrigger(object sender, EventArgs e)
    {
        currentPlayer.LockAllBindings();

        movingPlatform2AddInteractButton.OnInteractButtonRemoved +=
            MovingPlatform2AddInteractButton_OnInteractButtonRemoved;

        movingPlatformToStoreBlock.ChangeInteractionLockState(false);

        middleMovingPlatform2ApproximationTrigger.OnApproximationTrigger -=
            MiddleMovingPlatform2ApproximationTrigger_OnApproximationTrigger;
    }

    //Начало движения двигающейся платформы
    private void MovingPlatform2AddInteractButton_OnInteractButtonRemoved(object sender, EventArgs e)
    {
        movingPlatform2AddInteractButton.OnInteractButtonAdded +=
            MovingPlatform2AddInteractButton_OnInteractButtonAdded;

        movingPlatform2AddInteractButton.OnInteractButtonRemoved -=
            MovingPlatform2AddInteractButton_OnInteractButtonRemoved;
    }

    //Прибытие к точке финиша
    private void MovingPlatform2AddInteractButton_OnInteractButtonAdded(object sender, EventArgs e)
    {
        GuideInterface.Instance.Hide();
        GuideInterface.Instance.ShowGuide(breakableBlockBreakGuideSO);

        breakableBlock.ChangeIsCanBeStoredState(true);

        movingPlatformToStoreBlock.ChangeInteractionLockState(true);

        breakableBlockAddInteractButton.OnInteractButtonRemoved +=
            BreakableBlockAddInteractButton_OnInteractButtonRemoved1;

        movingPlatform2AddInteractButton.OnInteractButtonAdded -=
            MovingPlatform2AddInteractButton_OnInteractButtonAdded;
    }

    //Доставание ломаемого блока из двигающейся платформы
    private void BreakableBlockAddInteractButton_OnInteractButtonRemoved1(object sender, EventArgs e)
    {
        GuideInterface.Instance.Hide();
        movingPlatformToStoreBlock.ChangeInteractionLockState(false);

        currentPlayer.UnlockAllBindings();

        breakableBlockAddInteractButton.OnInteractButtonRemoved -=
            BreakableBlockAddInteractButton_OnInteractButtonRemoved1;
    }
}
