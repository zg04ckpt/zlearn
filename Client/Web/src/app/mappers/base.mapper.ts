export abstract class Mapper<I, O> {
    abstract map(param: I): O;
}