/// <summary>
/// Abstract class defining an object that can be dragged across the screen.
/// </summary>
public abstract class Draggable
{
    /// <summary>
    /// Collision area of the object.
    /// </summary>
    public RectangleF Hitbox;

    /// <summary>
    /// Position of the object.
    /// </summary>
    public PointF position;

    /// <summary>
    /// Indicates whether the object is being moved.
    /// </summary>
    public bool isMoving = false;

    /// <summary>
    /// Indicates whether the object is fixed and cannot be moved.
    /// </summary>
    public bool isFixed = false;

    /// <summary>
    /// Initial click position to calculate relative movement.
    /// </summary>
    internal PointF? initialClick = null;

    /// <summary>
    /// Constructor for the Draggable class.
    /// </summary>
    /// <param name="position">Initial position of the object.</param>
    /// <param name="width">Width of the object.</param>
    /// <param name="height">Height of the object.</param>
    public Draggable(PointF position, float width, float height)
    {
        this.position = position;
        this.Hitbox = new RectangleF(position.X, position.Y, width, height);
    }

    /// <summary>
    /// Handles the mouse click event when pressed.
    /// </summary>
    /// <param name="cursor">Mouse cursor position.</param>
    /// <returns>True if the click is within the object's collision area, otherwise False.</returns>
    public virtual bool HandleClickDown(PointF cursor)
    {
        if (!this.Hitbox.Contains(cursor))
        {
            isMoving = false;
            return false;
        }
        GetInitialClick(cursor);
        isMoving = true;
        return true;
    }

    /// <summary>
    /// Handles the mouse click event when released.
    /// </summary>
    /// <param name="cursor">Mouse cursor position.</param>
    public virtual void HandleClickUp(PointF cursor) => isMoving = false;

    /// <summary>
    /// Updates the object's position according to mouse movement.
    /// </summary>
    /// <param name="cursor">New mouse cursor position.</param>
    public void UpdatePosition(PointF cursor)
    {
        if (isMoving)
            position = GetRealPosition(cursor);
        UpdateHitbox();
    }

    /// <summary>
    /// Draws the object on the screen.
    /// </summary>
    /// <param name="g">Graphics object used for drawing.</param>
    public virtual void Draw(Graphics g) => g.DrawRectangle(Pens.Red, this.Hitbox);

    #region Private Methods

    private void GetInitialClick(PointF cursor)
    {
        var x = GetDistanceByAxis(position.X, cursor.X);
        var y = GetDistanceByAxis(position.Y, cursor.Y);
        initialClick = new PointF(x, y);
    }

    private void UpdateHitbox()
    {
        this.Hitbox.X = position.X;
        this.Hitbox.Y = position.Y;
    }

    private PointF GetRealPosition(PointF cursor)
    {
        float x,
            y;
        x = cursor.X - initialClick.Value.X;
        y = cursor.Y - initialClick.Value.Y;
        return new(x, y);
    }

    private float GetDistanceByAxis(float position1, float position2) =>
        (float)Math.Abs(position1 - position2);

    #endregion
}
