import {makeAutoObservable} from 'mobx';
//How to use Mobx
class CounterScore{
 title = 'Counter store';
 count = 0;
 events: string[] = [
    `Initial count is ${this.count}`
 ];
 constructor() {
    makeAutoObservable(this)
 }

 increment =( amount = 1) => {
    this.count += amount ;
    this.events.push(`Incremented by ${amount} - count is now  ${this.count}`);
 }
 decrement = (amount = 1) => {
    this.count -= amount; 
    this.events.push(`Decremented by ${amount} - count is now  ${this.count}`);
 }

 //computed property
 get eventCount(){
    return this.events.length;
 }
}
export default CounterScore;