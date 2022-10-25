import { TestBed } from "@angular/core/testing";
import { HttpClientTestingModule, HttpTestingController } from "@angular/common/http/testing";
import { ApiService } from "./api.service";
import { Category } from '../../models/category';

describe('ApiService', () => {
    let service: ApiService;
    let httpMock: HttpTestingController;
    

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [ApiService]
        })

        service = TestBed.get(ApiService);
        httpMock = TestBed.get(HttpTestingController);
    });

    afterEach(() => {
        httpMock.verify();
    })

    it('should GET category?id=0', () => {
        const dummyCategory: Category = {
            id: 0,
            name: "Category A",
            subcategoryIds: [
                0
            ]
        };

        service.getCategory(dummyCategory.id).subscribe((category) => {
            expect(category).toEqual(dummyCategory);
        })

        const request =  httpMock.expectOne(`${service.apiURL}/data/category?id=0`);
        
        expect(request.request.method).toBe('GET');

        request.flush(dummyCategory);
    });
});