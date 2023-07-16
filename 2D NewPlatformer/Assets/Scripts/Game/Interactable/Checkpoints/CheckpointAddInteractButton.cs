using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AddInteractButtonUI;

public class CheckpointAddInteractButton : AddInteractButtonUI
{
    protected override void Update()
    {
        if (IsAnySourceInteractable())
        {
            switch (interactCastForm)
            {
                case InteractCastForm.Default:
                    Vector3 castPosition = transform.position + (Vector3)collision.offset;
                    Vector2 castCubeLenght = collision.size;
                    float cubeRotation = 0f;
                    Vector2 cubeDirection = Vector2.up;

                    RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                        cubeRotation, cubeDirection, interactableHeight, playerLayer);
                    if (raycastHit)
                        if (raycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                        {
                            Checkpoint checkpoint = interactableItems[0] as Checkpoint;
                            if(checkpoint != null)
                                if(!checkpoint.GetIsWasPlacedAutomaticaly() && checkpoint.GetCheckpointPriority() >
                                    CheckpointsController.Instance.GetCurrentCheckpoint().GetCheckpointPriority())
                                {
                                    checkpoint.PlaceCheckpointAutomaticaly(interactedPlayer);
                                    return;
                                }
                            if (!isHasButtonOnInterface)
                                AddInteractButtonToInterafce();
                            return;
                        }

                    if (isHasButtonOnInterface)
                        RemoveInteractButtonFromInterafce();
                    break;
                case InteractCastForm.MovingPlatform:
                    castPosition = transform.position + new Vector3(0f, interactableHeight / 2, 0f);
                    castCubeLenght = new Vector2(collision.size.x, interactableHeight);
                    cubeRotation = 0f;
                    cubeDirection = Vector2.up;
                    float additionCubeLength = 0f;

                    raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                        cubeRotation, cubeDirection, additionCubeLength, playerLayer);
                    if (raycastHit)
                        if (raycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                        {
                            if (!isHasButtonOnInterface)
                                AddInteractButtonToInterafce();
                            return;
                        }

                    if (isHasButtonOnInterface)
                        RemoveInteractButtonFromInterafce();
                    break;
            }
        }
    }

    public override void OnInteract()
    {
        Checkpoint checkpoint = interactableItems[0] as Checkpoint;
        if (checkpoint != null)
        {
            checkpoint.OnInteract(interactedPlayer);
        }
        if(isHasButtonOnInterface)
            RemoveInteractButtonFromInterafce();
    }
}
