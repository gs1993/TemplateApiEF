export class ApiResponse {
    result: Object[];
    errorMessage: string;
    timeGenerated: string;

    Succeeded() {
        if (this.errorMessage != null) {
            return false;
        }
        return true;
    }
}
