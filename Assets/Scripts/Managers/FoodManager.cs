using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;
    [SerializeField] private Transform field;
    public List<Food> foodList = new List<Food>();
    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        for (int i = 0; i < 50; i++)
        {
            GameObject food = PoolManager.instance.GetFood();
            food.transform.position = SetPosition();
            foodList.Add(new Food(food, 500));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 SetPosition()
    {
        float offsetField = (field.transform.localScale.x / 2) - GameConstants.OFFSET_FIELD;
        Vector3 snakePosition = new Vector3(Random.Range(-offsetField, offsetField), 0, Random.Range(-offsetField, offsetField));
        return snakePosition;
    }

    public void PickUpFood(Food food)
    {
        food.foodObject.SetActive(false);
        foodList.Remove(food);
    }
}

public class Food
{
    public int points;
    public GameObject foodObject;
    public Vector2 foodPosition;

    public Food (GameObject foodObject, int points)
    {
        this.points = points;
        this.foodObject = foodObject;
        foodPosition = new Vector2(foodObject.transform.position.x, foodObject.transform.position.z);
    }
}
