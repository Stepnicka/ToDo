import React, { Component } from 'react'
import ToDoForm from './ToDoForm'
import ToDoMessageList from './ToDoMessageList'
import ToDoList from './ToDoList'

const baseUrl = 'http://localhost:49439'

export class ToDo extends Component {
    static displayName = ToDo.name;

    constructor(props) {
        super(props);
        this.state = { messages: [], tasks: [], states: [] };
    }

    componentDidMount() {
        /* Get possible states & saved tasks on INIT */
        this.callGetAll();
    }

    componentDidUpdate (){
        // console.log(this.state);
    }

    /// Add new task
    addTask = async (task) => {
        if(task.TaskName.trim().length < 5)
        {
            this.printMessages( ["Task name minimal lenght is 5 characters"]);
            return false;
        }

        if(isNumber(task.TaskPriority) === false || task.TaskPriority <= 0){
            this.printMessages( ["Minimal priority is 1"]);
            return false;
        }

        if(this.checkTaskNameExists(task.TaskName, 0) === true){
            this.printMessages( ["Task name already exists"]);
            return false;
        }

        const result = await this.callCreateTask(task);

        if(result.task === null){
            this.printMessages(result.messages);
            return false;
        }

        let newState = {...this.state}
        newState.messages = [];
        newState.tasks = [...this.state.tasks, result.task]
        this.setState(newState);
        
        return true;
    }

    /// Update task
    updateTask = async (task) => {
        if(task.name.trim().length < 5)
        {
            this.printMessages( ["Task name minimal lenght is 5 characters"]);
            return false;
        }

        if(isNumber(task.priority) === false || task.priority <= 0){
            this.printMessages( ["Minimal priority is 1"]);
            return false;
        }

        if(this.checkTaskNameExists(task.name, task.toDoTaskId) === true){
            this.printMessages( ["Task name already exists"]);
            return false;
        }

        let result = await this.callUpdateTask(task);

        if(result.success === false)
        {
            this.printMessages(result.messages);
            return false;
        }

        let newState = {...this.state}
        newState.messages = [];
        this.setState(newState);

        return true;
    }

    /* Delete existing task */
    deleteTask = async (task) => {
        if(task.taskStatusId !== 3){
            this.printMessages( ["Only completed tasks can be removed"]);
            return false;
        }

        const index = this.state.tasks.indexOf(task);

        const result = await this.callDeleteTask(task);

        if(result.success === false)
        {
            this.printMessages(result.messages);
            return false;
        }
        
        let newState = {...this.state}
        newState.tasks.splice(index, 1);
        newState.messages = [];

        this.setState(newState);

        return true;
    }

    /// Mark tasks as not edited
    markEdit = (toDoTaskId) =>{
        let newState = {...this.state}

        for(const task of newState.tasks){
            if(task.toDoTaskId === toDoTaskId){
                task.edited = true;
            }
            else{
                task.edited = false;
            }
        }

        this.setState(newState);
    }

    //// Validate if name exists
    checkTaskNameExists = (taskName, taskId) => {

        for(const task of this.state.tasks){
            if(taskName === task.name && taskId !== task.toDoTaskId){
                return true;
            }
        }

        return false;
    }

    /// Print fault message
    printMessages = (messages) =>{
        const final = this.createMessages(messages);

        let newState = {...this.state}
        newState.messages = final;

        console.log('setting messages');
        this.setState(newState);
    }

    /// Create message object
    createMessages = (messages) =>{
        let final = [];
        let index = 0;

        for (const item of messages) {
            final.push({Text: item, Key: index++ })
        }

        return final;
    }

    /*-------------------------------------------------------------------------------------*/
    render() {

        return (
            <div className=".container-sm">
                <ToDoMessageList messages={this.state.messages}/>
                <ToDoForm addTask={this.addTask}/>
                <ToDoList tasks={this.state.tasks} states={this.state.states} markEdit={this.markEdit} updateTask={this.updateTask} deleteTask={this.deleteTask}/>
            </div>
        );
    }
    /*-------------------------------------------------------------------------------------*/

    /* Get all states and tasks */
    async callGetAll() {
        const statesResult = await this.getAllStates();
        const taskResult = await this.getAllTasks();

        var errors = statesResult.messages.concat(taskResult.messages);
        errors = this.createMessages(errors);

        for(const task of taskResult.tasks)
            task.edited = false;

        const newState = {...this.state}
        newState.states = statesResult.states;
        newState.tasks = taskResult.tasks;
        newState.messages = errors;

        newState.states.sort(compareStates)

        this.setState(newState);
    }

    async getAllStates() {
        const result = await fetch(baseUrl+'/api/TaskStatus/GetAll');

        if(result.ok === true){
            const response = await result.json();
            return {states: response.states, messages: []};
        }
        else{
            const simpleError = await result.json();
            return {states: [], messages: simpleError.errors}
        }
    }

    async getAllTasks(){
        const result = await fetch(baseUrl+'/api/ToDoTask/GetAll');

        if(result.ok === true){
            const response = await result.json();
            return {tasks: response.tasks, messages: []};
        }
        else{
            const simpleError = await result.json();
            return {states: [], messages: simpleError.errors}
        }
    }

    /* Create new Task */
    async callCreateTask(task){
        const result = await fetch(baseUrl + '/api/ToDoTask/Create', 
        { 
            method: 'POST', 
            headers: {
                'content-type': 'application/json'
              },
            body: JSON.stringify({
                priority: task.TaskPriority,
                name: task.TaskName,
            }) 
        })

        if(result.ok === true){
            const response = await result.json();
            return {task: response.task, messages: []};
        }
        else{
            const simpleError = await result.json();
            return {task: null, messages: simpleError.errors}
        }
    }

    /* Update existing task */
    async callUpdateTask(task){
        const result = await fetch(baseUrl + '/api/ToDoTask/Update', 
        { 
            method: 'PUT', 
            headers: {
                'content-type': 'application/json'
              },
            body: JSON.stringify({
                task:{
                    toDoTaskId: task.toDoTaskId,
                    taskStatusId: task.taskStatusId,
                    priority: task.priority,
                    name: task.name
                }
            })
        })

        if(result.ok === true){
            return { success: true, messages: []};
        }
        else{
            const simpleError = await result.json();
            return { success: false, messages: simpleError.errors}
        }
    }

    /* Delete existing task */
    async callDeleteTask(task){
        const result = await fetch(baseUrl + '/api/ToDoTask/Delete', 
        { 
            method: 'DELETE', 
            headers: {
                'content-type': 'application/json'
              },
            body: JSON.stringify({
                toDoTaskId : task.toDoTaskId
            })
        })

        if(result.ok === true){
            return { success: true, messages: []};
        }
        else{
            const simpleError = await result.json();
            return { success: false, messages: simpleError.errors}
        }
    }
}

function isNumber(n) { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }

function compareStates( a, b ) {
    if ( a.taskStatusId < b.taskStatusId ){
      return -1;
    }
    if ( a.taskStatusId > b.taskStatusId ){
      return 1;
    }
    return 0;
  }