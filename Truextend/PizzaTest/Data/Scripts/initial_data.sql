USE PizzeriaDB;

-- Pizza table Data
INSERT INTO Pizza (Id, Name, SmallImageUrl, LargeImageUrl, Description)
VALUES
  ('EC54A954-012C-4778-A1FD-8C79E2EF16C2', 'Capricciosa', 'https://drive.google.com/uc?export=view&id=1KQXwkqGkzgrz4b1RzGFM3N6J_KpA-b0C', 'https://drive.google.com/uc?export=view&id=1emwlaGPweIU2QM34G4f0Di75VmLTbKE2','A delectable combination of savory toppings on a thin and crispy crust.'),
  ('AAAE98C7-1D24-4D8F-A59B-CA7B09EC3A51', 'Ham & Cheese', 'https://drive.google.com/uc?export=view&id=1o_NMkfk0gkACt2mTtW7ogtjwq56Oq8e9', 'https://drive.google.com/uc?export=view&id=1eprTNUSZNCLPfIjmUqpQF8HeyxnMt9e0', 'The perfect blend of smoky ham and gooey melted cheese on a flavorful crust.'),
  ('81620D43-F173-4318-A186-9681DC77DC91', 'Hawaiian', 'https://drive.google.com/uc?export=view&id=1PILgmdVqaP_E-xxsZbAPj6NTTsQbJqcz', 'https://drive.google.com/uc?export=view&id=1MKJTSiPYoID-gPSbGu3dPyEyPiQr7A5r', 'Experience a tropical twist with the harmonious blend of sweet pineapple and savory ham.'),
  ('25C88CDD-F747-4ABA-825D-BE964E600AF8', 'Margherita', 'https://drive.google.com/uc?export=view&id=1w7hETceBaLPH1_gMtdaRRyK5oqNdYHh2', 'https://drive.google.com/uc?export=view&id=1Ewg3Ctoe_wFdGyjbTg9NhvkDwtPRRVrh', 'A timeless classic, featuring a harmony of tangy tomatoes, fresh mozzarella, and aromatic basil.'),
  ('BAB06802-8711-4D71-AF67-AAB233340337', 'Meat lovers', 'https://drive.google.com/uc?export=view&id=1hen2X07agidxIZdBbFy_Qpj0ZCEJgM62', 'https://drive.google.com/uc?export=view&id=15s2sPRqkkJ5RodtSX0JsXaaJy9QDF7nJ', 'Indulge in a carnivorous delight with an abundance of mouthwatering meats on every slice.'),
  ('B7FE535D-0855-44D2-BAA8-066A1C15B39C', 'Pepperoni', 'https://drive.google.com/uc?export=view&id=1rnMZhhabkWqAW0h51kCOZnJuMLM52hjF', 'https://drive.google.com/uc?export=view&id=1pvbyfiXYLrSB_IiKach9doROZ-WlhgSA', 'The ultimate pizza staple, with zesty pepperoni that delivers a burst of flavor in every bite.'),
  ('5CD89B39-D4DA-4956-89C4-1A482889F273', 'Quattro Formaggi', 'https://drive.google.com/uc?export=view&id=1g_VKXMkW-7OP9dsOruCnekm_u9JD83XJ', 'https://drive.google.com/uc?export=view&id=1cnXeszH0lngyol_zP2wzNWD0z4yPNa-t', 'Dive into a cheesy paradise with a quartet of exquisite cheeses melting together.'),
  ('0F20267C-4125-4FA4-8053-24D299DA0579', 'Veggie', 'https://drive.google.com/uc?export=view&id=1LBg3kRNDmmgK0XeseCPOebZQzZBw7EGb', 'https://drive.google.com/uc?export=view&id=14Fc4UL7dobyehNUz4MxYViXp7PBkBv9n', 'Dive into a cheesy paradise with a quartet of exquisite cheeses melting together.');

-- ToppingTable
INSERT INTO Topping (Id, Name)
VALUES
  ('17F0181B-D488-412F-B3C2-65E841134C44', 'Tomato'),
  ('29EC7E38-EBFD-402B-9768-0628310A14F3', 'Mozzarella cheese'),
  ('23FB593B-72CD-45FA-8D0C-880D2C7A1644', 'Fontina cheese'),
  ('F1B03B5B-B64B-4C87-AC6D-9A84FCF8F7A5', 'Gorgonzola cheese'),
  ('7FA1C857-788C-4898-9657-457258C1468D', 'Parmigiano-Reggiano cheese'),
  ('C301A0AF-551E-438F-9221-EF35E1E0D893', 'Basil'),
  ('6D0B7106-3C1F-4470-82AC-19115249F681', 'Champignol'),
  ('E9B10C50-02BF-4FE9-B066-1A7F07A76032', 'Mushrooms'),
  ('A0D02611-7968-43E2-8145-4C6C102874EE', 'Olives'),
  ('7D89D2E2-C39D-4AAF-9DE1-98FFEE264128', 'Onions'),
  ('FD25EC6F-E86B-4345-AB6D-EFC4B83DF2DB', 'Artichokes'),
  ('82CBF9E0-7550-4092-8705-FAA15E3C3952', 'Pineapple'),
  ('1725BE6F-6A37-4F9B-AAFB-3F8A54A04E73', 'Ham'),
  ('CA704309-7F8F-49E5-8928-1A57BF01D9EB', 'Pepperoni'),
  ('D73E9D0A-C0A9-4B74-A02A-D22BCA5975AF', 'Sausage'),
  ('A3659E30-8112-4037-9361-DE38033A781B', 'Bacon');

-- PizzaTopings Table
INSERT INTO PizzaTopping (PizzaId, ToppingId)
VALUES
  -- Toppings for 'Capricciosa'
  ('EC54A954-012C-4778-A1FD-8C79E2EF16C2', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('EC54A954-012C-4778-A1FD-8C79E2EF16C2', '1725BE6F-6A37-4F9B-AAFB-3F8A54A04E73'), -- Ham
  ('EC54A954-012C-4778-A1FD-8C79E2EF16C2', 'E9B10C50-02BF-4FE9-B066-1A7F07A76032'), -- Mushrooms
  ('EC54A954-012C-4778-A1FD-8C79E2EF16C2', 'FD25EC6F-E86B-4345-AB6D-EFC4B83DF2DB'), -- Artichokes
  ('EC54A954-012C-4778-A1FD-8C79E2EF16C2', 'A0D02611-7968-43E2-8145-4C6C102874EE'), -- Olives
  
  -- Toppings for 'Ham & Cheese'
  ('AAAE98C7-1D24-4D8F-A59B-CA7B09EC3A51', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('AAAE98C7-1D24-4D8F-A59B-CA7B09EC3A51', '1725BE6F-6A37-4F9B-AAFB-3F8A54A04E73'), -- Ham

-- Toppings for 'Hawaiian'
  ('81620D43-F173-4318-A186-9681DC77DC91', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('81620D43-F173-4318-A186-9681DC77DC91', '1725BE6F-6A37-4F9B-AAFB-3F8A54A04E73'), -- Ham
  ('81620D43-F173-4318-A186-9681DC77DC91', '82CBF9E0-7550-4092-8705-FAA15E3C3952'), -- Pineapple

-- Toppings for 'Margherita'
  ('25C88CDD-F747-4ABA-825D-BE964E600AF8', '17F0181B-D488-412F-B3C2-65E841134C44'), -- Tomato
  ('25C88CDD-F747-4ABA-825D-BE964E600AF8', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('25C88CDD-F747-4ABA-825D-BE964E600AF8', 'C301A0AF-551E-438F-9221-EF35E1E0D893'), -- Basil

-- Toppings for 'Meat lovers'
  ('BAB06802-8711-4D71-AF67-AAB233340337', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('BAB06802-8711-4D71-AF67-AAB233340337', '1725BE6F-6A37-4F9B-AAFB-3F8A54A04E73'), -- Ham
  ('BAB06802-8711-4D71-AF67-AAB233340337', 'D73E9D0A-C0A9-4B74-A02A-D22BCA5975AF'), -- Sausage
  ('BAB06802-8711-4D71-AF67-AAB233340337', 'A3659E30-8112-4037-9361-DE38033A781B'), -- Bacon

-- Toppings for 'Pepperoni'
  ('B7FE535D-0855-44D2-BAA8-066A1C15B39C', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('B7FE535D-0855-44D2-BAA8-066A1C15B39C', 'CA704309-7F8F-49E5-8928-1A57BF01D9EB'), -- Pepperoni

-- Toppings for 'Quattro Formaggi'
  ('5CD89B39-D4DA-4956-89C4-1A482889F273', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('5CD89B39-D4DA-4956-89C4-1A482889F273', '23FB593B-72CD-45FA-8D0C-880D2C7A1644'), -- Fontina cheese
  ('5CD89B39-D4DA-4956-89C4-1A482889F273', 'F1B03B5B-B64B-4C87-AC6D-9A84FCF8F7A5'), -- Gorgonzola cheese
  ('5CD89B39-D4DA-4956-89C4-1A482889F273', '7FA1C857-788C-4898-9657-457258C1468D'), -- Parmigiano-Reggiano cheese

-- Toppings for 'Veggie'
  ('0F20267C-4125-4FA4-8053-24D299DA0579', '17F0181B-D488-412F-B3C2-65E841134C44'), -- Tomato
  ('0F20267C-4125-4FA4-8053-24D299DA0579', '29EC7E38-EBFD-402B-9768-0628310A14F3'), -- Mozzarella cheese
  ('0F20267C-4125-4FA4-8053-24D299DA0579', 'C301A0AF-551E-438F-9221-EF35E1E0D893'), -- Basil
  ('0F20267C-4125-4FA4-8053-24D299DA0579', '6D0B7106-3C1F-4470-82AC-19115249F681'), -- Champignol
  ('0F20267C-4125-4FA4-8053-24D299DA0579', 'E9B10C50-02BF-4FE9-B066-1A7F07A76032'), -- Mushrooms
  ('0F20267C-4125-4FA4-8053-24D299DA0579', 'A0D02611-7968-43E2-8145-4C6C102874EE'), -- Olives
  ('0F20267C-4125-4FA4-8053-24D299DA0579', '7D89D2E2-C39D-4AAF-9DE1-98FFEE264128'); -- Onions
