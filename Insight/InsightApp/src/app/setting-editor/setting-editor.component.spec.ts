import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingEditorComponent } from './setting-editor.component';

describe('SettingEditorComponent', () => {
  let component: SettingEditorComponent;
  let fixture: ComponentFixture<SettingEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SettingEditorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettingEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
