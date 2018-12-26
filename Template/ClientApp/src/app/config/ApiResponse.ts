export class ApiResponse {
    data: Object[];
    errorMessage: string;
    timeGenerated: string;

    Succeeded() {
        if (this.errorMessage != null) {
            return false;
        }
        return true;
    }
}
