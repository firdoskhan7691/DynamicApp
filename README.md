# DynamicApp
## Endpoint: CreateProgram
HTTP Method: POST
### Route: /CreateProgram

### Description
This endpoint is responsible for creating a new program. It accepts an ApplicationFormModel object containing the details of the program to be created. The program's ID is automatically generated and assigned within the method.

Request
The request must include a JSON object with the following properties:

ProgramName (string): The name of the program. This field is required and cannot be null or empty.
ProgramDesc (string): A description of the program. This field is required and cannot be null or empty.
Request Example

```
{
  "programId": "",
  "programName": "Uk Training",
  "programDesc": "Uk Training",
  "formFields": [
    {
      "fieldName": "FirstName",
      "isMandatory": true,
      "isHidden": false,
      "isInternal": false
    },
    {
      "fieldName": "LaststName",
      "isMandatory": true,
      "isHidden": false,
      "isInternal": true
    },
    {
      "fieldName": "Email",
      "isMandatory": true,
      "isHidden": true,
      "isInternal": true
    }
  ],
  "questions": [
    {
      "questionId": "",
      "programId": "",
      "questionTypeId": "1",
      "questionType": "Dropdown",
      "question": "Please selct country",
      "choices": [
        "Indai",
        "Japan",
        "US"
      ]
    },
    {
      "questionId": "",
      "programId": "",
      "questionTypeId": "2",
      "questionType": "MultipleChoice",
      "question": "Please select at least 2 answers from dropdown",
      "choices": [
        "Playing",
        "Music",
        "Travelling"
      ]
    },
    {
      "questionId": "",
      "programId": "",
      "questionTypeId": "3",
      "questionType": "YesOrNo",
      "question": "Have you ever been rejcted by UK embassy?",
      "choices": [
        "Yes",
        "No"
      ]
    },
     {
      "questionId": "",
      "programId": "",
      "questionTypeId": "4",
      "questionType": "Paragraph",
      "question": "Plesae tell me about your self",
      "choices": []
    },
    {
      "questionId": "",
      "programId": "",
      "questionTypeId": "5",
      "questionType": "Number",
      "question": "How many years of experiance you have",
      "choices": []
    },
    {
      "questionId": "",
      "programId": "",
      "questionTypeId": "6",
      "questionType": "Date",
      "question": "Please provide your last working day",
      "choices": []
    }
  ]
}
```

## Response

The endpoint returns various HTTP status codes based on the outcome of the request:

200 OK: Returned when the program is successfully created. The response body contains a success message.

400 Bad Request: Returned when the request contains invalid data. The response body contains an error message describing the issue.

## Cosmos Document In Program Container:
```
{
    "id": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
    "programName": "Uk Training",
    "programDesc": "Uk Training",
    "formFields": [
        {
            "fieldName": "FirstName",
            "isMandatory": true,
            "isHidden": false,
            "isInternal": false
        },
        {
            "fieldName": "LaststName",
            "isMandatory": true,
            "isHidden": false,
            "isInternal": true
        },
        {
            "fieldName": "Email",
            "isMandatory": true,
            "isHidden": true,
            "isInternal": true
        }
    ],
    "_rid": "BQotALKt5FUBAAAAAAAAAA==",
    "_self": "dbs/BQotAA==/colls/BQotALKt5FU=/docs/BQotALKt5FUBAAAAAAAAAA==/",
    "_etag": "\"00000000-0000-0000-a891-ea5c4bea01da\"",
    "_attachments": "attachments/",
    "_ts": 1715974761
}
```
## Cosmos Document In Question Container:

```
{
    "id": "5fb628a1-d725-4397-88a8-ba9681e4ead7",
    "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
    "questionTypeId": "4",
    "questionType": "Paragraph",
    "question": "Plesae tell me about your self",
    "choices": [],
    "_rid": "BQotAIpD5IIBAAAAAAAAAA==",
    "_self": "dbs/BQotAA==/colls/BQotAIpD5II=/docs/BQotAIpD5IIBAAAAAAAAAA==/",
    "_etag": "\"00000000-0000-0000-a891-ea9078d301da\"",
    "_attachments": "attachments/",
    "_ts": 1715974761
}
```

## Endpoint: GetProgram
### HTTP Method: GET
### Route: /GetProgram/{programId}
### Description
This endpoint retrieves the details of a program by its ID. It returns the program details if the program is found; otherwise, it returns a not found response.
This end point will be used by UI team to display/create dynamic page i.e. screen 2, where candidate fill his information and apply.

Request
The request must include the programId as a route parameter.

Request Example
```
GET /GetProgram/12345
```

### Response
The endpoint returns various HTTP status codes based on the outcome of the request:

200 OK: Returned when the program is successfully retrieved. The response body contains the program details.

404 Not Found: Returned when no program is found with the given ID. The response body contains an error message.

400 Bad Request: Returned when there is an exception during the operation. The response body contains the exception message.

### Response Example:
```
{
    "questions": [
        {
            "questionId": "5fb628a1-d725-4397-88a8-ba9681e4ead7",
            "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
            "questionTypeId": "4",
            "questionType": "Paragraph",
            "question": "Plesae tell me about your self",
            "choices": []
        },
        {
            "questionId": "effe7254-0233-49de-80bd-65e6b4a4b4ec",
            "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
            "questionTypeId": "1",
            "questionType": "Dropdown",
            "question": "Please selct country",
            "choices": [
                "Indai",
                "Japan",
                "US"
            ]
        },
        {
            "questionId": "c6162a3e-7b74-40d8-a8ac-30277a857a4a",
            "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
            "questionTypeId": "5",
            "questionType": "Number",
            "question": "How many years of experiance you have",
            "choices": []
        },
        {
            "questionId": "c2437451-2d04-4af4-b332-e09e11e99a83",
            "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
            "questionTypeId": "6",
            "questionType": "Date",
            "question": "Please provide your last working day",
            "choices": []
        },
        {
            "questionId": "b24ecec3-8f2a-4c47-8a2e-a9c75837d0a2",
            "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
            "questionTypeId": "3",
            "questionType": "YesOrNo",
            "question": "Have you ever been rejcted by UK embassy?",
            "choices": [
                "Yes",
                "No"
            ]
        },
        {
            "questionId": "976b36ef-870a-4916-b263-761df594be1a",
            "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
            "questionTypeId": "2",
            "questionType": "MultipleChoice",
            "question": "Please select at least 2 answers from dropdown",
            "choices": [
                "Playing",
                "Music",
                "Travelling",
                "Swimming"
            ]
        }
    ],
    "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
    "programName": "Uk Training",
    "programDesc": "Uk Training",
    "formFields": [
        {
            "fieldName": "FirstName",
            "isMandatory": true,
            "isHidden": false,
            "isInternal": false
        },
        {
            "fieldName": "LaststName",
            "isMandatory": true,
            "isHidden": false,
            "isInternal": true
        },
        {
            "fieldName": "Email",
            "isMandatory": true,
            "isHidden": true,
            "isInternal": true
        }
    ]
}
```

## Endpoint: AddQuestion
### HTTP Method: POST
### Route: /AddQuestion
### Description
This endpoint is responsible for adding a new question. It accepts a QuestionModel object containing the details of the question to be added. The question's ID is automatically generated and assigned within the method.

### Request
The request must include a JSON object with the following properties:

### Request Example
```
{
  "questionId": "",
  "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
  "questionTypeId": "6",
  "questionType": "RadioButton",
  "question": "Are you ready to relocate",
  "choices": [
    "Yes",
    "No"
  ]
}
```

### Response
The endpoint returns various HTTP status codes based on the outcome of the request:

200 OK: Returned when the question is successfully added. The response body contains a success message.

400 Bad Request: Returned when there is an exception during the operation. The response body contains the exception message.

## Endpoint: GetQuestions
### HTTP Method: GET
### Route: /GetQuestions/{programId}
### Description
This endpoint retrieves a list of questions associated with a specific program ID. It returns the list of questions if any are found; otherwise, it returns a not found response.

### Request
The request must include the programId as a route parameter.

Request Example
```
GET /GetQuestions/12345
```

### Response
The endpoint returns various HTTP status codes based on the outcome of the request:

200 OK: Returned when the questions are successfully retrieved. The response body contains the list of questions.

404 Not Found: Returned when no questions are found for the given program ID. The response body contains an error message.

400 Bad Request: Returned when there is an exception during the operation. The response body contains the exception message.
### Response Example
```
[
    {
        "questionId": "5fb628a1-d725-4397-88a8-ba9681e4ead7",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "4",
        "questionType": "Paragraph",
        "question": "Plesae tell me about your self",
        "choices": []
    },
    {
        "questionId": "effe7254-0233-49de-80bd-65e6b4a4b4ec",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "1",
        "questionType": "Dropdown",
        "question": "Please selct country",
        "choices": [
            "Indai",
            "Japan",
            "US"
        ]
    },
    {
        "questionId": "c6162a3e-7b74-40d8-a8ac-30277a857a4a",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "5",
        "questionType": "Number",
        "question": "How many years of experiance you have",
        "choices": []
    },
    {
        "questionId": "c2437451-2d04-4af4-b332-e09e11e99a83",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "6",
        "questionType": "Date",
        "question": "Please provide your last working day",
        "choices": []
    },
    {
        "questionId": "b24ecec3-8f2a-4c47-8a2e-a9c75837d0a2",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "3",
        "questionType": "YesOrNo",
        "question": "Have you ever been rejcted by UK embassy?",
        "choices": [
            "Yes",
            "No"
        ]
    },
    {
        "questionId": "976b36ef-870a-4916-b263-761df594be1a",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "2",
        "questionType": "MultipleChoice",
        "question": "Please select at least 2 answers from dropdown",
        "choices": [
            "Playing",
            "Music",
            "Travelling",
            "Swimming"
        ]
    }
]
```

## Endpoint: UpdateQuestion
### HTTP Method: PUT
### Route: /UpdateQuestion
### Description
This endpoint is responsible for updating an existing question. It accepts a QuestionModel object containing the updated details of the question.

### Request
The request must include a JSON object with the following properties:
~~~
 {
        "questionId": "976b36ef-870a-4916-b263-761df594be1a",
        "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
        "questionTypeId": "2",
        "questionType": "MultipleChoice",
        "question": "Please select at least 2 answers from dropdown",
        "choices": [
            "Playing",
            "Music",
            "Travelling",
            "Swimming"
        ]
    }
~~~

### Response
The endpoint returns various HTTP status codes based on the outcome of the request:

200 OK: Returned when the question is successfully updated.

400 Bad Request: Returned when there is an exception during the operation. The response body contains the exception message.
### Response Example
~~~
{
  "message": "Question Updated Successfully"
}
~~~

## Endpoint: DeleteQuestion
### HTTP Method: DELETE
### Route: /DeleteQuestion/{questionId}
### Description
This endpoint deletes a question by its ID. It returns a success response if the question is successfully deleted; otherwise, it returns an error response.

### Request
The request must include the questionId as a route parameter.

### Request Example
~~~
DELETE /DeleteQuestion/1
~~~

## Endpoint: AddCandidateApplication
### HTTP Method: POST
### Route: /AddCandidateApplication
### Description
This endpoint is responsible for adding a new candidate application. It accepts a CandidateModel object containing the details of the candidate to be added. The candidate's ID is automatically generated and assigned within the method.

### Request
The request must include a JSON object with the following properties:
### Request Example
~~~
{
  "candidateId": "",
  "programId": "da60e3d0-a803-41f0-a010-6acdc5e2f13c",
  "firstName": "Firdos",
  "lastName": "Khan",
  "email": "firdos.khan49@gmail.com",
  "phone": "9097622020",
  "nationalty": "Indain",
  "currentResidence": "Hyd",
  "idNumber": "1234",
  "dateOfBirth": "07-06-1991",
  "gender": "Male",
  "candidateAnswers": [
    {
      "questionId": "9eb440aa-f14b-4e6d-8443-3a646d75e1ff",
      "questionType": "YesOrNo",
      "ansewer": "Yes"
    },
    {
      "questionId": "976b36ef-870a-4916-b263-761df594be1a",
      "questionType": "MultipleChoice",
      "ansewer": "Playing,Music"
    }
  ]
}
~~~

### Response
The endpoint returns various HTTP status codes based on the outcome of the request:

200 OK: Returned when the candidate application is successfully added. The response body contains a success message.

400 Bad Request: Returned when the request contains invalid data. The response body contains an error message describing the issue.

### Response Example
~~~
{
  "message": "Candidate Application Added Successfully"
}
~~~
