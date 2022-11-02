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

        service = TestBed.inject(ApiService);
        httpMock = TestBed.inject(HttpTestingController); // Mocks requests
    });

    afterEach(() => {
        httpMock.verify();
    })

    it('should create', () => { 
        expect(service).toBeTruthy();
    })

    it('should getCategory', () => {
        const dummyCategory: Category = {
            id: 0,
            name: "Category A",
            subcategoryIds: [
                0
            ]
        };
        service.getCategory(dummyCategory.id).subscribe((category) => {
            expect(category).toEqual(dummyCategory); // Validate that output of getCategory method matches dummyCategory
        })
        const request = httpMock.expectOne(`${service.apiURL}/data/category?id=0`); // Mock a request to endpoint
        expect(request.request.method).toBe('GET'); // Validate request is a GET request
        request.flush(dummyCategory); // Resolve request with mock data
    });
});