using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public GameObject menuPanel;
    public GameObject victoryPanel;


    // References to TextMeshPro components
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] optionTexts;

    // References to left door and left light objects
    public GameObject leftDoor;
    public GameObject leftLight;

    public GameObject rightDoor;
    public GameObject rightLight;

    public GameObject leftEnemy;
    public GameObject rightEnemy;

    public GameObject jumpscareObjects;

    // Array to store questions, options, and correct answers
    [System.Serializable]
    public struct Question
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
    }
    private Dictionary<string, Question[]> subjectQuestions;

    public string[] subjects = { "Science", "Math", "Geography", "French", "History" };
    public TMP_Dropdown subjectDropdown;
    private int selectedSubjectIndex = 0;

    public Question[] questions = { };

    private int currentQuestionIndex = 0;

    public AudioManager audioManager;

    private bool inGame = false;

    private float enemyMovementSpeed = 0.001f;
    private float enemyOriginalZ = -2f;

    void Start()
    {
        InitializeQuestions();
        subjectDropdown.ClearOptions();
        subjectDropdown.AddOptions(new List<string>(subjects));
        StartGame();
    }

    void InitializeQuestions()
    {
        subjectQuestions = new Dictionary<string, Question[]>
        {
            {"Science", new Question[]
                {
                    new Question { question = "What is the chemical symbol for gold?", options = new string[] { "A) Au", "B) Ag", "C) Fe", "D) Cu" }, correctAnswerIndex = 0 },
                    new Question { question = "Which planet is known as the 'Red Planet'?", options = new string[] { "A) Mars", "B) Venus", "C) Jupiter", "D) Saturn" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the largest organ in the human body?", options = new string[] { "A) Skin", "B) Liver", "C) Heart", "D) Brain" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the chemical formula for water?", options = new string[] { "A) H2O", "B) CO2", "C) NaCl", "D) CH4" }, correctAnswerIndex = 0 },
                    new Question { question = "Which of these is not a state of matter?", options = new string[] { "A) Energy", "B) Solid", "C) Liquid", "D) Gas" }, correctAnswerIndex = 0 }
                }
            },
            {"Math", new Question[]
                {
                    new Question { question = "What is the result of 8 x 7?", options = new string[] { "A) 56", "B) 54", "C) 58", "D) 62" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the square root of 144?", options = new string[] { "A) 12", "B) 14", "C) 16", "D) 18" }, correctAnswerIndex = 0 },
                    new Question { question = "What is 3² + 4²?", options = new string[] { "A) 25", "B) 49", "C) 16", "D) 81" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the next number in the sequence: 2, 4, 8, 16, ...?", options = new string[] { "A) 32", "B) 24", "C) 28", "D) 20" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the value of π (pi) to two decimal places?", options = new string[] { "A) 3.14", "B) 3.16", "C) 3.12", "D) 3.18" }, correctAnswerIndex = 0 }
                }
            },
            {"Geography", new Question[]
                {
                    new Question { question = "What is the capital of France?", options = new string[] { "A) Paris", "B) London", "C) Berlin", "D) Rome" }, correctAnswerIndex = 0 },
                    new Question { question = "Which is the largest continent by land area?", options = new string[] { "A) Asia", "B) Africa", "C) Europe", "D) Antarctica" }, correctAnswerIndex = 0 },
                    new Question { question = "Which river is the longest in the world?", options = new string[] { "A) Nile", "B) Amazon", "C) Yangtze", "D) Mississippi" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the largest country by land area?", options = new string[] { "A) Russia", "B) Canada", "C) China", "D) USA" }, correctAnswerIndex = 0 },
                    new Question { question = "Which mountain range runs along South America's western coast?", options = new string[] { "A) Andes", "B) Rockies", "C) Alps", "D) Himalayas" }, correctAnswerIndex = 0 }
                }
            },
            {"French", new Question[]
                {
                    new Question { question = "What is 'hello' in French?", options = new string[] { "A) Bonjour", "B) Merci", "C) Au revoir", "D) S'il vous plaît" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the French word for 'cat'?", options = new string[] { "A) Chat", "B) Chien", "C) Oiseau", "D) Poisson" }, correctAnswerIndex = 0 },
                    new Question { question = "How do you say 'yes' in French?", options = new string[] { "A) Oui", "B) Non", "C) Peut-être", "D) Jamais" }, correctAnswerIndex = 0 },
                    new Question { question = "What is the French word for 'water'?", options = new string[] { "A) Eau", "B) Feu", "C) Terre", "D) Air" }, correctAnswerIndex = 0 },
                    new Question { question = "How do you say 'goodbye' in French?", options = new string[] { "A) Au revoir", "B) Bonjour", "C) Merci", "D) S'il vous plaît" }, correctAnswerIndex = 0 }
                }
            },
            {"History", new Question[]
                {
                    new Question { question = "In which year did Christopher Columbus first reach the Americas?", options = new string[] { "A) 1492", "B) 1776", "C) 1066", "D) 1812" }, correctAnswerIndex = 0 },
                    new Question { question = "Who was the first President of the United States?", options = new string[] { "A) Washington", "B) Jefferson", "C) Lincoln", "D) Adams" }, correctAnswerIndex = 0 },
                    new Question { question = "In which year did World War II end?", options = new string[] { "A) 1945", "B) 1939", "C) 1918", "D) 1941" }, correctAnswerIndex = 0 },
                    new Question { question = "Who was the leader of the Soviet Union during most of World War II?", options = new string[] { "A) Stalin", "B) Lenin", "C) Trotsky", "D) Khrushchev" }, correctAnswerIndex = 0 },
                    new Question { question = "Which ancient wonder was located in Alexandria, Egypt?", options = new string[] { "A) Lighthouse", "B) Pyramid", "C) Garden", "D) Statue" }, correctAnswerIndex = 0 }
                }
            }
        };
    }

    void LoadQuestionsForSubject(int subjectIndex)
    {
        string selectedSubject = subjects[subjectIndex];
        if (subjectQuestions.TryGetValue(selectedSubject, out Question[] loadedQuestions))
        {
            questions = loadedQuestions;
            Debug.Log($"Loaded {questions.Length} questions for subject: {selectedSubject}");
        }
        else
        {
            Debug.LogError($"No questions found for subject: {selectedSubject}");
        }
    }

    public void StartGame()
    {
        quizPanel.SetActive(false);
        victoryPanel.SetActive(false);
        menuPanel.SetActive(true);
        // Reset enemy positions
        leftEnemy.transform.position = new Vector3(leftEnemy.transform.position.x, leftEnemy.transform.position.y, enemyOriginalZ);
        rightEnemy.transform.position = new Vector3(rightEnemy.transform.position.x, rightEnemy.transform.position.y, enemyOriginalZ);
    }


    void Update()
    {
        if (inGame)
        {
            leftEnemy.transform.position += new Vector3(0, 0, enemyMovementSpeed);
            rightEnemy.transform.position += new Vector3(0, 0, enemyMovementSpeed);

            // Make sure the enemy is always facing the player
            leftEnemy.transform.LookAt(Camera.main.transform.position);
            rightEnemy.transform.LookAt(Camera.main.transform.position);

            if ((leftEnemy.transform.position.z > 0 || rightEnemy.transform.position.z > 0))
            {
                ActivateJumpscare();
            }
        }
    }


    public void StartQuiz()
    {
        inGame = true;
        quizPanel.SetActive(true);
        menuPanel.SetActive(false);
        currentQuestionIndex = 0;  // Reset the question index

        // Remove existing listeners to prevent duplicates
        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].GetComponentInParent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        }

        // Add new listeners
        for (int i = 0; i < optionTexts.Length; i++)
        {
            int optionIndex = i;
            optionTexts[i].GetComponentInParent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(optionIndex));
        }

        // Load questions for the selected subject
        LoadQuestionsForSubject(subjectDropdown.value);

        // Display first question
        DisplayQuestion();
    }

    void CheckAnswer(int selectedOption)
    {
        if (currentQuestionIndex >= questions.Length)
        {
            Debug.LogError("Attempted to check answer after quiz completion");
            return;
        }

        if (selectedOption == questions[currentQuestionIndex].correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            audioManager.PlayAudioClip("CorrectAnswer");

            currentQuestionIndex++;

            if (currentQuestionIndex < questions.Length)
            {
                DisplayQuestion();
            }
            else
            {
                Debug.Log("Quiz complete!");
                quizComplete();
            }
        }
        else
        {
            Debug.Log("Incorrect answer!");
            audioManager.PlayAudioClip("IncorrectAnswer");
            // You might want to add some penalty here, like moving the enemies closer
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            questionText.text = questions[currentQuestionIndex].question;
            for (int i = 0; i < optionTexts.Length; i++)
            {
                optionTexts[i].text = questions[currentQuestionIndex].options[i];
            }
        }
        else
        {
            Debug.LogError("Attempted to display question beyond quiz length");
            quizComplete();
        }
    }

    public void quizComplete()
    {
        inGame = false;
        quizPanel.SetActive(false);
        victoryPanel.SetActive(true);
        audioManager.PlayAudioClip("NightComplete");
        StartCoroutine(DeactivateVictoryPanel());
    }

    IEnumerator DeactivateVictoryPanel()
    {
        yield return new WaitForSeconds(5f);
        victoryPanel.SetActive(false);
        StartGame();
    }

    // Method to handle left door button click
    public void OnLeftDoorButtonClick()
    {
        audioManager.PlayAudioClip("DoorClose"); // Play a sound effect (you need to add this audio clip to the AudioManager script

        leftDoor.SetActive(!leftDoor.activeSelf); // Toggle the active state of the left door object
    }

    // Method to handle left light button click
    public void OnLeftLightButtonClick()
    {
        audioManager.PlayAudioClip("LightClose"); // Play a sound effect (you need to add this audio clip to the AudioManager script

        leftLight.SetActive(!leftLight.activeSelf); // Toggle the active state of the left light object
    }

    // Method to handle right door button click
    public void OnRightDoorButtonClick()
    {
        audioManager.PlayAudioClip("DoorClose"); // Play a sound effect (you need to add this audio clip to the AudioManager script

        rightDoor.SetActive(!rightDoor.activeSelf); // Toggle the active state of the right door object
    }

    // Method to handle right light button click
    public void OnRightLightButtonClick()
    {
        audioManager.PlayAudioClip("LightClose"); // Play a sound effect (you need to add this audio clip to the AudioManager script

        rightLight.SetActive(!rightLight.activeSelf); // Toggle the active state of the right light object
    }

    public void ActivateJumpscare()
    {
        inGame = false;

        audioManager.PlayAudioClip("Jumpscare"); // Play a sound effect (you need to add this audio clip to the AudioManager script

        jumpscareObjects.SetActive(true);
        quizPanel.SetActive(false);

        StartCoroutine(DeactivateJumpscare());
    }

    IEnumerator DeactivateJumpscare()
    {
        yield return new WaitForSeconds(3f);
        jumpscareObjects.SetActive(false);
        StartGame();
    }

}
